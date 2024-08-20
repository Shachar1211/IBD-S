namespace finalproj.BL
{
    public class UserParams
    {
        private int percentageOfDisability;
        private bool disabilityRating;
        private bool student;
        private string ibdType;
        private int userId;
        public UserParams(int percentageOfDisability, bool disabilityRating, string ibdType, int userId, bool student)
        {
            PercentageOfDisability = percentageOfDisability;
            DisabilityRating = disabilityRating;
            IbdType = ibdType;
            UserId = userId;
            Student = student; 
        }

        public UserParams()
        {
        }
        public int PercentageOfDisability { get => percentageOfDisability; set => percentageOfDisability = value; }
        public bool DisabilityRating { get => disabilityRating; set => disabilityRating = value; }
        public string IbdType { get => ibdType; set => ibdType = value; }
        public int UserId { get => userId; set => userId = value; }
        public bool Student { get => student; set => student = value; }

        public string ParamsToQuestion()
        {
            string question = "";
            if (DisabilityRating == true)
            {
                question = $"אחוזי נכות, ומקבל קצבת נכות {PercentageOfDisability} בעל ,{IbdType} אני חולה";

            }
            else
            {
                question = $"אחוזי נכות {PercentageOfDisability} בעל ,{IbdType} אני חולה";
            }
            if (Student == true)
            {
                question += "ואני סטודנט";
            }
            return question;
        }
    }
}