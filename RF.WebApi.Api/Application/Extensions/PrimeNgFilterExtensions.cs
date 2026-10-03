using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RF.WebApi.Api.Application.DTOs.Common;

namespace RF.WebApi.Api.Application.Extensions
{
    public static class PrimeNgFilterExtensions
    {
        public static async Task<RF.WebApi.Api.Application.DTOs.Common.PagedResult<T>> ApplyPrimeNgAsync<T>(
            this IQueryable<T> query,
            TableLazyLoadEventDto request,
            CancellationToken cancellationToken = default)
        {
            // 0. Global Filter (Search Term)
            if (!string.IsNullOrWhiteSpace(request.GlobalFilter))
            {
                // This is a generic approach for string properties. 
                // Alternatively, you can pass a specific search expression or fields.
                var stringProperties = typeof(T).GetProperties()
                    .Where(p => p.PropertyType == typeof(string))
                    .Select(p => p.Name);

                if (stringProperties.Any())
                {
                    var searchClauses = new List<string>();
                    foreach (var prop in stringProperties)
                    {
                        searchClauses.Add($"{prop}.Contains(@0)");
                    }
                    var combinedSearch = string.Join(" or ", searchClauses);
                    query = query.Where(combinedSearch, request.GlobalFilter);
                }
            }

            // 1. Dynamic Filtering
            if (request.Filters != null && request.Filters.Count > 0)
            {
                var filterClauses = new List<string>();
                var filterParams = new List<object>();
                int paramIndex = 0;

                foreach (var kvp in request.Filters)
                {
                    var field = kvp.Key;
                    
                    // PrimeNG sends global filter in the 'Filters' object as well sometimes (key is "global"). 
                    // We've handled it above, so skip if present here
                    if (field.ToLower() == "global") continue;

                    var metaList = kvp.Value;
                    if (metaList == null || metaList.Length == 0) continue;

                    var columnSubClauses = new List<string>();

                    foreach (var meta in metaList)
                    {
                        if (meta.Value == null || string.IsNullOrWhiteSpace(meta.Value.ToString()))
                            continue;

                        object? filterValue = meta.Value;
                        if (filterValue is System.Text.Json.JsonElement je)
                        {
                            switch (je.ValueKind)
                            {
                                case System.Text.Json.JsonValueKind.String:
                                    filterValue = je.GetString();
                                    break;
                                case System.Text.Json.JsonValueKind.Number:
                                    if (je.TryGetInt32(out int i)) filterValue = i;
                                    else filterValue = je.GetDouble();
                                    break;
                                case System.Text.Json.JsonValueKind.True:
                                    filterValue = true;
                                    break;
                                case System.Text.Json.JsonValueKind.False:
                                    filterValue = false;
                                    break;
                                default:
                                    filterValue = je.ToString();
                                    break;
                            }
                        }

                        var expression = BuildExpression(field, meta.MatchMode, paramIndex);
                        if (!string.IsNullOrEmpty(expression))
                        {
                            columnSubClauses.Add(expression);
                            filterParams.Add(filterValue!);
                            paramIndex++;
                        }
                    }

                    if (columnSubClauses.Count > 0)
                    {
                        var joined = string.Join(" or ", columnSubClauses);
                        filterClauses.Add($"({joined})");
                    }
                }

                if (filterClauses.Count > 0)
                {
                    var combinedWhere = string.Join(" and ", filterClauses);
                    query = query.Where(combinedWhere, filterParams.ToArray());
                }
            }

            // 2. Count Total Records Before Paging
            var totalRecords = await query.CountAsync(cancellationToken);

            // 3. Dynamic Sorting
            if (!string.IsNullOrWhiteSpace(request.SortField))
            {
                var direction = request.SortOrder == -1 ? "descending" : "ascending";
                query = query.OrderBy($"{request.SortField} {direction}");
            }

            // 4. Pagination
            int skip = request.First ?? 0;
            int take = request.Rows ?? 10;
            
            List<T> data;
            if (take == -1)
            {
                data = await query.Skip(skip).ToListAsync(cancellationToken);
            }
            else
            {
                data = await query.Skip(skip).Take(take).ToListAsync(cancellationToken);
            }

            return new RF.WebApi.Api.Application.DTOs.Common.PagedResult<T>
            {
                Data = data,
                TotalRecords = totalRecords
            };
        }

        private static string BuildExpression(string field, string? matchMode, int idx) => matchMode?.ToLower() switch
        {
            "startswith" => $"{field}.StartsWith(@{idx})",
            "endswith" => $"{field}.EndsWith(@{idx})",
            "contains" => $"{field}.Contains(@{idx})",
            "notcontains" => $"!{field}.Contains(@{idx})",
            "equals" => $"{field} == @{idx}",
            "notequals" => $"{field} != @{idx}",
            "gt" => $"{field} > @{idx}",
            "gte" => $"{field} >= @{idx}",
            "lt" => $"{field} < @{idx}",
            "lte" => $"{field} <= @{idx}",
            _ => $"{field} == @{idx}"
        };
    }
}
