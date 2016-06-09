using ADPQ.Entities.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ADPQ.Data
{
    internal class ADPQContext: DbContext
    {
        public ADPQContext()
            : base(ConfigurationManager.ConnectionStrings["ConnName"].ConnectionString)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Person_Name> Person_Names { get; set; }
        public DbSet<Address> Addresss { get; set; }
        public DbSet<PhoneTypes> PhoneType { get; set; }
		public DbSet<MessageModel> Message { get; set; }
        public DbSet<RelationshipProof> ChildRelationship { get; set; }
        public DbSet<ChildDisability> ChildDisability { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<RelationshipMap> RelationshipMaps { get; set; }
        public DbSet<PersonProfilePic> PersonProfilePics { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("DM_USER").HasKey(x=>x.USER_ID);
            modelBuilder.Entity<Person>().ToTable("DM_PERSON").HasKey(x=>x.PERS_ID);
            modelBuilder.Entity<Person_Name>().ToTable("DM_PERSON_NAME").HasKey(x=>x.PERS_NM_ID);
            modelBuilder.Entity<Address>().ToTable("DM_ADDRESS").HasKey(x => x.ADR_ID);
            modelBuilder.Entity<PhoneTypes>().ToTable("DM_Phone_Types").HasKey(x => x.PHON_TYP_ID);
			modelBuilder.Entity<MessageModel>().ToTable("INBOX").HasKey(x => x.Message_ID);
            modelBuilder.Entity<RelationshipProof>().ToTable("DM_RELATIONSHIPPROOF").HasKey(x => x.Relationship_Proof_ID);
            modelBuilder.Entity<ChildDisability>().ToTable("DM_DISABILITY").HasKey(x => x.DIS_ID);
            modelBuilder.Entity<Token>().ToTable("Token").HasKey(x => x.TokenId);
            modelBuilder.Entity<RelationshipMap>().ToTable("DM_RELATIONSHIP_MAP").HasKey(x => x.RELATIONSHIP_MAP_ID);
            modelBuilder.Entity<PersonProfilePic>().ToTable("DM_PERSON_PROFILEPIC").HasKey(x => x.Profile_File_Id);
        }
    }
}
