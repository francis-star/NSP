using HCWeb2016; 

namespace DAL
{
    public class DAL_KeyWordDts : SqlBase
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="km_ProcinceCode"></param>
        /// <param name="km_ProcinceName"></param>
        /// <param name="km_CityCode"></param>
        /// <param name="km_CItyName"></param>
        /// <param name="km_Name"></param>
        /// <param name="joinman"></param>
        /// <returns></returns>
        public bool SaveWord(string km_ProvinceCode, string km_Name, string km_PlatForm, int danger, string joinman)
        {
            string str = @"EXEC [dbo].[SP_PLInsertKeyWords] '" + km_ProvinceCode + "','" + km_Name + "','" + km_PlatForm + "'," + danger + ",'" + joinman + "'";
            return UpdateData(str);
        }
    }
}
