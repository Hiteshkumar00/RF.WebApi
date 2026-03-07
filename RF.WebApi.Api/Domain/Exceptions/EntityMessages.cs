namespace RF.WebApi.Api.Domain.Exceptions
{
    public struct EntityMessages
    {
        public const string NotFound = "Entity details not found.";
        public const string AlreadyExists = "Entity already exists.";
        public const string InvalidData = "Invalid entity data provided.";
    }

    public struct RelatedEntityMessages
    {
        public const string NotFound = "Related Entity details not found.";
        public const string AlreadyExists = "Related Entity already exists.";
        public const string InvalidData = "Invalid related entity data provided.";
    }
}
