namespace RestBasicProject
{
    /// <summary>
    /// This is poco for Posts of URL :"http://jsonplaceholder.typicode.com/"
    /// </summary>
    public class Posts
    {
        /// <summary>
        /// This is variable to set user id of Posts
        /// </summary>
        public int userId { get; set; }

        /// <summary>
        /// This is variable to set id of Posts
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// This is variable to set title of Posts
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// This is variable to set body of Posts
        /// </summary>
        public string body { get; set; }
    }
}
