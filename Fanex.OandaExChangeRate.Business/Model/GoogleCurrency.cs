namespace Fanex.OandaExChangeRate.Business.Model
{
    using System;

    public class GoogleCurrency
    {
        private string _lhs;
        private string _rhs;
        

        public string lhs
        {
            get
            {
                
                RemoveSpecialChar(ref _lhs);
                return _lhs;
            }
            set { _lhs = value.Trim(); }
        }
        public string @rhs
        {
            get
            {
               
                RemoveSpecialChar(ref _rhs);
                return _rhs;
            }
            set
            {
                _rhs = value.Trim();
            }
        }
        public string error { get; set; }
        public bool icc { get; set; }

        public void RemoveSpecialChar(ref string strSource)
        {
            strSource = strSource.Replace(Convert.ToChar(160).ToString(), "");
            strSource = strSource.Replace("&nbsp;", "");
            strSource = strSource.Replace('�'.ToString(), "");
        }
    }
}
