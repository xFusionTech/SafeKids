using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ADPQ.Entities.Model
{
    public class User
    {
        public User() { }

        
        public Guid USER_ID { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public Guid PERS_ID { get; set; }
        public int QUESTION_1 { get; set; }
        public string ANSWER_1 { get; set; }
        public int QUESTION_2 { get; set; }
        public string ANSWER_2 { get; set; }
        [NotMapped]
        public string OLDPASSWORD { get; set; }
        [NotMapped]
        public bool MAILING_ADR { get; set; }
        [NotMapped]
        public Person Person { get; set; }
        [NotMapped]
        public Person_Name personname { get; set; }
        [NotMapped]
        public RelationshipMap RelationshipMap { get; set; }
        [NotMapped]
        public List<RelationshipProof> child_relationship { get; set; }
        [NotMapped]
        public List<ChildDisability> child_disability { get; set; }
        [NotMapped]
        public List<Address> Address { get; set; }
        [NotMapped]
        public List<PhoneTypes> PhoneTypes { get; set; }
        [NotMapped]
        public Guid TokenId { get; set; }
        [NotMapped]
        public Guid ChildID { get; set; }
        [NotMapped]
        public List<PersonProfilePic> PersonProfilePic { get; set; }
        public User(Guid USER_ID, string USERNAME, string PASSWORD, Guid PERS_ID, string ANSWER_1, int QUESTION_1, string ANSWER_2, int QUESTION_2)
        {
            this.USER_ID = USER_ID;
            this.USERNAME = USERNAME;
            this.PASSWORD = PASSWORD;
            this.PERS_ID = PERS_ID;
            this.QUESTION_1 = QUESTION_1;
            this.QUESTION_2 = QUESTION_1;
            this.ANSWER_1 = ANSWER_1;
            this.ANSWER_2 = ANSWER_2;



        }
    }
}
