using ADPQ.Entities.Model;
using ADPQ.Data.Contract;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ADPQ.Data.Repository
{
    public class ChildRepository : ChildRepositoryContract
    {
        List<RelationshipProof> childRLT = new List<RelationshipProof>();
        List<ChildDisability> childDis = new List<ChildDisability>();

        public Guid Add(User child)
        {
            try
            {

                Guid PersonID = Guid.NewGuid();
                using (var db = new ADPQContext())
                {
                    Person objperson = new Person
                    {
                        PERS_ID = PersonID,
                        Gender = child.Person.Gender,
                        Race_Ethencity = child.Person.Race_Ethencity,
                        DOB = child.Person.DOB,
                        IsParent = false,
                        Social_Security_No = child.Person.Social_Security_No
                    };
                    db.Persons.Add(objperson);
                    // db.SaveChanges();

                    Guid PersonNameID = Guid.NewGuid();
                    Person_Name ObjPersonName = new Person_Name
                    {
                        PERS_FNAME = child.personname.PERS_FNAME,
                        PERS_LNAME = child.personname.PERS_LNAME,
                        PERS_MNAME = child.personname.PERS_MNAME,
                        PERS_NM_ID = PersonNameID,
                        PERS_ID = PersonID
                    };
                    db.Person_Names.Add(ObjPersonName);
                    //  db.SaveChanges();

                    foreach (var item in child.child_disability)
                    {
                        Guid CHILD_DIS_ID = Guid.NewGuid();
                        ChildDisability objChildDis = new ChildDisability
                        {
                            DIS_PERS_ID = PersonID,
                            DIS_ID = CHILD_DIS_ID,
                            DIS_TypeId = item.DIS_TypeId,
                            DIS_UploadDoc_TypeId = item.DIS_UploadDoc_TypeId,
                            DIS_Document = item.DIS_Document,
                            NOTE = item.NOTE,
                            DIS_File_Name = item.DIS_File_Name

                        };
                        db.ChildDisability.Add(objChildDis);
                        db.SaveChanges();
                    }

                    foreach (var item in child.child_relationship)
                    {
                        Guid CHILD_RLT_ID = Guid.NewGuid();
                        RelationshipProof objRLT = new RelationshipProof
                        {                            
                            Relationship_Proof_ID = CHILD_RLT_ID,
                            Primary_Person_ID = child.PERS_ID,       //primary - Parent , Secondory -CHiLD
                            Secondary_Person_ID = PersonID,
                            //  RLTCODE = item.RLTCODE,
                            Proof_Type = item.Proof_Type,
                            Proof_Doc = item.Proof_Doc,
                            Proof_File_Name = item.Proof_File_Name
                        };
                        db.ChildRelationship.Add(objRLT);
                        db.SaveChanges();
                    }


                    RelationshipMap ObjRelationshipMap = new RelationshipMap
                    {
                        RELATIONSHIP_MAP_ID=Guid.NewGuid(),
                        Primary_Person_ID = child.PERS_ID,
                        Secondary_Person_ID = PersonID,
                        Relationship_Code = child.RelationshipMap.Relationship_Code

                    };

                    foreach (var item in child.PersonProfilePic)
                    {
                        Guid Profile_FileId = Guid.NewGuid();
                        PersonProfilePic ObjPersonProfilePic = new PersonProfilePic
                        {
                            Profile_File_Id= Profile_FileId,
                            PERS_ID = PersonID,
                            DIS_PIC_ID = item.DIS_PIC_ID,
                            Profile_File_Name = item.Profile_File_Name

                        };
                        db.PersonProfilePics.Add(ObjPersonProfilePic);
                        db.SaveChanges();
                    }

                    db.RelationshipMaps.Add(ObjRelationshipMap);
                    db.SaveChanges();


                }
                return PersonID;
            }
            catch (Exception ex)
            {
                return new Guid();
            }
        }

        public bool Edit(User child)
        {
            try
            {
                Person ObjPer;
                Person_Name ObjPersonName;
                List<RelationshipProof> ObjChildRLT;
                List<ChildDisability> ObjChildDis;
                List<RelationshipMap> ObjRelationshipMap;
                List<PersonProfilePic> ObjPersonProfilePic;
                using (var db = new ADPQContext())
                {
                    ObjPer = db.Persons.FirstOrDefault(u => u.PERS_ID.Equals(child.ChildID));
                    ObjPersonName = db.Person_Names.FirstOrDefault(u => u.PERS_ID.Equals(child.ChildID));
                    ObjChildRLT = db.ChildRelationship.Where(u => u.Secondary_Person_ID.Equals(child.ChildID)).ToList();
                    ObjPersonProfilePic = db.PersonProfilePics.Where(u => u.PERS_ID.Equals(child.ChildID)).ToList();
                    ObjChildDis = db.ChildDisability.Where(u => u.DIS_PERS_ID.Equals(child.ChildID)).ToList();
                    ObjRelationshipMap = db.RelationshipMaps.Where(u => u.Secondary_Person_ID.Equals(child.ChildID)).ToList();
                    if (ObjChildRLT.Count > 0)
                    {
                        db.ChildRelationship.RemoveRange(ObjChildRLT);
                        db.SaveChanges();
                    }
                    if (ObjPersonProfilePic.Count > 0)
                    {
                        db.PersonProfilePics.RemoveRange(ObjPersonProfilePic);
                        db.SaveChanges();
                    }
                    if (ObjChildDis.Count > 0)
                    {

                        db.ChildDisability.RemoveRange(ObjChildDis);
                        db.SaveChanges();

                    }
                    if (ObjRelationshipMap != null)
                    {
                        db.RelationshipMaps.RemoveRange(ObjRelationshipMap);
                        db.SaveChanges();
                    }
                }
                RelationshipMap ObjRelationshipMap1 = new RelationshipMap
                {
                    RELATIONSHIP_MAP_ID = Guid.NewGuid(),
                    Primary_Person_ID = child.PERS_ID,
                    Secondary_Person_ID = child.ChildID,
                    Relationship_Code = child.RelationshipMap.Relationship_Code

                };
                if (ObjPer != null)
                {
                    ObjPer.Social_Security_No = child.Person.Social_Security_No;
                    ObjPer.Gender = child.Person.Gender;
                    ObjPer.Race_Ethencity = child.Person.Race_Ethencity;
                    ObjPer.DOB = child.Person.DOB;
                    ObjPer.IsParent = false;
                }
                if (ObjPersonName != null)
                {
                    ObjPersonName.PERS_FNAME = child.personname.PERS_FNAME;
                    ObjPersonName.PERS_LNAME = child.personname.PERS_LNAME;
                    ObjPersonName.PERS_MNAME = child.personname.PERS_MNAME;
                }
                //if (ObjRelationshipMap != null)
                //{
                //    ObjRelationshipMap.Relationship_Code = child.RelationshipMap.Relationship_Code;
                //}

                foreach (var item in child.PersonProfilePic)
                {
                    Guid Profile_FileId = Guid.NewGuid();
                    PersonProfilePic ObjPersonProfilePics = new PersonProfilePic
                    {
                        Profile_File_Id= Profile_FileId,
                        PERS_ID = child.ChildID,
                        DIS_PIC_ID = item.DIS_PIC_ID,
                        Profile_File_Name = item.Profile_File_Name

                    };
                    using (var db = new ADPQContext())
                    {
                        db.PersonProfilePics.Add(ObjPersonProfilePics);
                        db.SaveChanges();
                    }
                }

                using (var dbCtx = new ADPQContext())
                {

                    dbCtx.RelationshipMaps.Add(ObjRelationshipMap1);
                    dbCtx.Entry(ObjPer).State = System.Data.Entity.EntityState.Modified;
                    dbCtx.Entry(ObjPersonName).State = System.Data.Entity.EntityState.Modified;
                    dbCtx.SaveChanges();
                }

                using (var db = new ADPQContext())
                {
                    foreach (var item in child.child_relationship)
                    {
                        Guid CHILD_RLT_ID = Guid.NewGuid();
                        RelationshipProof objRLT = new RelationshipProof
                        {
                            Relationship_Proof_ID = CHILD_RLT_ID,
                            Primary_Person_ID = child.PERS_ID,
                            Secondary_Person_ID = child.ChildID,
                            //RLTCODE = item.RLTCODE,
                            Proof_Type = item.Proof_Type,
                            Proof_Doc = item.Proof_Doc,
                            Proof_File_Name = item.Proof_File_Name

                        };
                        db.ChildRelationship.Add(objRLT);
                        db.SaveChanges();
                    }
                }

                using (var db = new ADPQContext())
                {
                    foreach (var item in child.child_disability)
                    {
                        Guid CHILD_DIS_ID = Guid.NewGuid();
                        ChildDisability objChildDis = new ChildDisability
                        {
                            DIS_PERS_ID = child.ChildID,
                            DIS_ID = CHILD_DIS_ID,
                            DIS_TypeId = item.DIS_TypeId,
                            DIS_UploadDoc_TypeId = item.DIS_UploadDoc_TypeId,
                            DIS_Document = item.DIS_Document,
                            NOTE = item.NOTE,
                            DIS_File_Name = item.DIS_File_Name
                        };
                        db.ChildDisability.Add(objChildDis);
                        db.SaveChanges();
                    }


                }

                return true;
            }
            catch (Exception Ex)
            {
                return false;
            }
        }

        public User Get(Guid PERS_ID,Guid personId)
        {
            try
            {
                CommonRepository ObjCommon = new CommonRepository();
                List<PersonProfilePic> ObjPersonProfilePic = new List<PersonProfilePic>();
                User UserDetail = new User();
                using (var db = new ADPQContext())
                {
                    var res = (from u in db.RelationshipMaps
                               where u.Secondary_Person_ID.Equals(PERS_ID)
                               select new
                               {
                                   personname = (from p in db.Person_Names
                                                 where p.PERS_ID.Equals(PERS_ID)
                                                 select p).FirstOrDefault(),
                                   person = (from p in db.Persons
                                             where p.PERS_ID.Equals(PERS_ID)
                                             select p).FirstOrDefault(),
                                   relationship = (from a in db.ChildRelationship
                                                   where a.Secondary_Person_ID.Equals(PERS_ID)
                                                   select a).ToList(),
                                   disability = (from ph in db.ChildDisability
                                                 where ph.DIS_PERS_ID.Equals(PERS_ID)
                                                 select ph).ToList(),

                                   RelationshipMap = (from rm in db.RelationshipMaps
                                                      where rm.Secondary_Person_ID.Equals(PERS_ID)
                                                      select rm).FirstOrDefault(),
                                   PersonProfilePic = (from rm in db.PersonProfilePics
                                                       where rm.PERS_ID.Equals(PERS_ID)
                                                       select rm).ToList(),


                               }).AsEnumerable().Select(x => new User
                               {
                                   personname = x.personname,
                                   Person = x.person,
                                   child_relationship = x.relationship,
                                   child_disability = x.disability,
                                   RelationshipMap = x.RelationshipMap,
                                   PersonProfilePic = x.PersonProfilePic

                               }).ToList();
                    UserDetail = res.FirstOrDefault();
                    List<ProfilePic> ImageList = new List<ProfilePic>();

                    foreach (var item in UserDetail.PersonProfilePic)
                    {
                        string MediaType = (item.DIS_PIC_ID.Split('.')[1].ToLower().Equals("png")) ? "image/png" : (item.DIS_PIC_ID.Split('.')[1].ToLower().Equals("image/jpg")) ? "image/jpg" : "image/jpeg";
                        ProfilePic ab = new ProfilePic
                        {
                             Image = ObjCommon.getImage(personId, item.PERS_ID, item.DIS_PIC_ID, MediaType),
                             DIS_PIC_ID = item.DIS_PIC_ID

                        };

                      ImageList.Add(ab);
                    }
                    ObjPersonProfilePic = (from a in UserDetail.PersonProfilePic
                             join b in ImageList
                             on a.DIS_PIC_ID equals b.DIS_PIC_ID
                             select new
                             {
                                 Profile_File_Id = a.Profile_File_Id,
                                 PERS_ID=a.PERS_ID,
                                 DIS_PIC_ID=a.DIS_PIC_ID,
                                 Profile_File_Name=a.Profile_File_Name,
                                 Image=b.Image                               

                             }).AsEnumerable().Select(x => new PersonProfilePic
                             {
                                 Profile_File_Id = x.Profile_File_Id,
                                 PERS_ID = x.PERS_ID,
                                 DIS_PIC_ID = x.DIS_PIC_ID,
                                 Profile_File_Name = x.Profile_File_Name,
                                 image = x.Image
                             }).ToList();

                }
                UserDetail.PersonProfilePic = ObjPersonProfilePic;
                return UserDetail;
            }
            catch (Exception)
            {
                throw;
            }
        }
      
        public List<RelationshipMap> GetChildList(Guid PERS_ID)
        {           

            List<RelationshipMap> ChildList = new List<RelationshipMap>();
            List<RelationshipMap> result = new List<RelationshipMap>();
            using (var db = new ADPQContext())
            {
                ChildList = db.RelationshipMaps.Where(x => x.Primary_Person_ID == PERS_ID).ToList();
                Person_Name PersonName = new Person_Name();
                List<Person_Name> PersonNameList = new List<Person_Name>();
                foreach (var item in ChildList)
                {
                    PersonName = db.Person_Names.Where(x => x.PERS_ID == PERS_ID).FirstOrDefault();
                    if (PersonName != null)
                    {
                        PersonNameList.Add(PersonName);
                    }


                }

                result = (from a in db.RelationshipMaps
                          join b in db.Person_Names
                          on a.Secondary_Person_ID equals b.PERS_ID
                          where a.Primary_Person_ID == PERS_ID
                          select new
                          {
                              personname = b,
                              Relationship_Code = a.Relationship_Code

                          }).AsEnumerable().Select(x => new RelationshipMap
                          {
                              personname = x.personname,
                              Relationship_Code = x.Relationship_Code
                          }).ToList();


            }

            return result;
        }

        public bool Delete(string filename, string Type)
        {
            using (var db = new ADPQContext())
            {              
                
                if (Type.Equals("Disability"))
                {
                 var res = db.ChildDisability.Where(x => x.DIS_Document.Equals(filename)).ToList();
                    db.ChildDisability.RemoveRange(res);
                }
                else if (Type.Equals("Relationship"))
                {
                    var res = db.ChildRelationship.Where(x => x.Proof_Doc.Equals(filename)).ToList();
                    db.ChildRelationship.RemoveRange(res);
                }
                else
                {
                    var res = db.PersonProfilePics.Where(x => x.DIS_PIC_ID.Equals(filename)).ToList();
                    db.PersonProfilePics.RemoveRange(res);
                }
                db.SaveChanges();
                return true;
            }

        }

       
    }
}