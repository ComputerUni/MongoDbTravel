namespace Travel.Web.Settings
{
    public interface IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string BannerCollectionName { get; set; }
        public string TourCollectionName { get; set; }
        public string CategoryCollectionName { get; set; }
        public string DestinationCollectionName { get; set; }
        public string ReservationCollectionName { get; set; }
        public string UserCollectionName { get; set; }
        public string CommentCollectionName { get; set; }
        public string QuestionCollectionName { get; set; }
        public string FavoriteCollectionName { get; set; }
        public string LookupItemCollectionName { get; set; }
        public string WhyUsItemCollectionName { get; set; }
        public string ContactCollectionName { get; set; }

    }
}
