namespace Fanex.Service.Core.Model
{
    public class Result
    {
        public Result()
        {
            ErrCode = 0;
            ErrMsg = string.Empty;
        }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}
