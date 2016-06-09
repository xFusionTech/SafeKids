using ADPQ.Data.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADPQ.Entities.Model;
using System.Data.Entity.Core.Objects;
using System.Configuration;

namespace ADPQ.Data.Repository
{
    public class UserRepository : UserRepositoryContract
    {
        public UserRepository()
        {
            //var dbcontext = new Context.XfusionDbContext();
        }

        public User login(User User)
        {
            User UserDetail = new User();

            using (var db = new ADPQContext())
            {
                var AuthUser = db.Users.FirstOrDefault(u => u.USERNAME == User.USERNAME && u.PASSWORD == User.PASSWORD);
                if (AuthUser != null)
                {
                    var res = (from u in db.Users
                               where u.PERS_ID == AuthUser.PERS_ID
                               select new
                               {
                                   PERS_ID = u.PERS_ID,
                                   USERNAME = u.USERNAME,
                                   person = (from p in db.Person_Names
                                             where p.PERS_ID.Equals(AuthUser.PERS_ID)
                                             select p).FirstOrDefault()

                               }).AsEnumerable().Select(x => new User
                               {
                                   PERS_ID = x.PERS_ID,
                                   USERNAME = x.USERNAME,
                                   personname = x.person
                               }).ToList();
                    UserDetail = res.FirstOrDefault();
                    UserDetail.TokenId = CreateToken(UserDetail.PERS_ID);
                }
            }
            return UserDetail;
        }

        public User getUserData(Guid PERS_ID, Guid Token)
        {
            User UserDetail = new User();

            using (var db = new ADPQContext())
            {
                var AuthUser = db.Users.FirstOrDefault(u => u.PERS_ID == PERS_ID);
                if (AuthUser != null)
                {
                    var res = (from u in db.Users
                               where u.PERS_ID == AuthUser.PERS_ID
                               select new
                               {
                                   PERS_ID = u.PERS_ID,
                                   USERNAME = u.USERNAME,
                                   person = (from p in db.Person_Names
                                             where p.PERS_ID.Equals(AuthUser.PERS_ID)
                                             select p).FirstOrDefault()

                               }).AsEnumerable().Select(x => new User
                               {
                                   PERS_ID = x.PERS_ID,
                                   USERNAME = x.USERNAME,
                                   personname = x.person
                               }).ToList();
                    UserDetail = res.FirstOrDefault();
                    UserDetail.TokenId = Token;
                }
            }
            return UserDetail;
        }

        //GetUserProfile(new Guid("89A1C60F-6487-4758-B9A6-B85A0CF5CE2C"));
        public Guid Register(User NewUser)
        {
            try
            {
                Guid PersonID = Guid.NewGuid();


                using (var db = new ADPQContext())
                {
                    Person objperson = new Person
                    {
                        PERS_ID = PersonID,
                        Gender = NewUser.Person.Gender,
                        Social_Security_No = NewUser.Person.Social_Security_No,
                        DOB = DateTime.Now,
                        email = NewUser.Person.email,
                        Race_Ethencity = 0,
                        TIMETOCALL = NewUser.Person.TIMETOCALL
                    };
                    db.Persons.Add(objperson);
                    db.SaveChanges();

                    Guid PersonNameID = Guid.NewGuid();
                    Person_Name ObjPersonName = new Person_Name
                    {

                        PERS_FNAME = NewUser.personname.PERS_FNAME,
                        PERS_LNAME = NewUser.personname.PERS_LNAME,
                        PERS_MNAME = string.IsNullOrEmpty(NewUser.personname.PERS_MNAME) ? "" : NewUser.personname.PERS_MNAME,
                        PERS_NAME_SFIX = NewUser.personname.PERS_NAME_SFIX,
                        PERS_NAME_PFIX = NewUser.personname.PERS_NAME_PFIX,
                        PERS_NM_ID = PersonNameID,
                        PERS_ID = PersonID

                    };
                    db.Person_Names.Add(ObjPersonName);
                    db.SaveChanges();

                    foreach (var item in NewUser.Address)
                    {
                        Guid AddressID = Guid.NewGuid();
                        Address Add = new Address
                        {
                            ADR_ID = AddressID,
                            ADR_LINE1 = item.ADR_LINE1,
                            ADR_LINE2 = item.ADR_LINE2,
                            ADR_CITY = item.ADR_CITY,
                            ADR_STATE_CD = item.ADR_STATE_CD,
                            // ADR_CNTRY = item.ADR_CNTRY,
                            ADR_ZIP_CD = item.ADR_ZIP_CD,
                            ADR_ZIP_EXTN = item.ADR_ZIP_EXTN,

                            //  LOC_ID = item.LOC_ID,
                            MAILING_ADR = item.MAILING_ADR,
                            ADR_DESC = item.ADR_DESC,
                            PERS_ID = PersonID
                        };
                        db.Addresss.Add(Add);
                        db.SaveChanges();
                        if (NewUser.MAILING_ADR)
                        {
                            break;
                        }

                    }

                    foreach (var item in NewUser.PhoneTypes)
                    {
                        Guid PhoneTypeId = Guid.NewGuid();
                        PhoneTypes ObjPhoneTypes = new PhoneTypes
                        {
                            PHON_TYP_ID = PhoneTypeId,
                            PHON_TYPE_CD = item.PHON_TYPE_CD,
                            PHON_TYP_DESC = item.PHON_TYP_DESC,
                            PERS_ID = PersonID

                        };
                        db.PhoneType.Add(ObjPhoneTypes);
                        db.SaveChanges();

                    }
                    foreach (var item in NewUser.PersonProfilePic)
                    {
                        Guid Profile_FileId = Guid.NewGuid();
                        PersonProfilePic ObjPersonProfilePic = new PersonProfilePic
                        {
                            Profile_File_Id = Profile_FileId,
                            PERS_ID = PersonID,
                            DIS_PIC_ID = item.DIS_PIC_ID,
                            Profile_File_Name = item.Profile_File_Name

                        };
                        db.PersonProfilePics.Add(ObjPersonProfilePic);
                        db.SaveChanges();
                        string path = ConfigurationManager.AppSettings["UploadPath"];
                        bool res = MoveTempFile(path + PersonID, path + item.DIS_PIC_ID, path + PersonID + "\\" + item.DIS_PIC_ID);
                    }

                    Guid UserId = Guid.NewGuid();
                    User ObjUser = new User
                    {
                        USER_ID = UserId,
                        USERNAME = NewUser.USERNAME,
                        PASSWORD = NewUser.PASSWORD,
                        QUESTION_1 = NewUser.QUESTION_1,
                        QUESTION_2 = NewUser.QUESTION_2,
                        ANSWER_1 = NewUser.ANSWER_1,
                        ANSWER_2 = NewUser.ANSWER_2,
                        PERS_ID = PersonID
                    };
                    db.Users.Add(ObjUser);
                    db.SaveChanges();


                }

                Guid Token = CreateToken(PersonID);
                return Token;
            }
            catch (Exception ex)
            {
                return new Guid();
            }

        }
        public bool UpdateProfile(User UpdateUser)
        {
            try
            {


                Person ObjPer;
                User ObjUser;
                Person_Name ObjPersonName;
                List<Address> ObjAddress;
                List<PhoneTypes> ObjPhone;
                List<PersonProfilePic> ObjPersonProfilePic;
                using (var db = new ADPQContext())
                {
                    ObjPer = db.Persons.FirstOrDefault(u => u.PERS_ID.Equals(UpdateUser.PERS_ID));
                    ObjUser = db.Users.FirstOrDefault(u => u.PERS_ID.Equals(UpdateUser.PERS_ID));
                    ObjPersonName = db.Person_Names.FirstOrDefault(u => u.PERS_ID.Equals(UpdateUser.PERS_ID));
                    ObjPersonProfilePic = db.PersonProfilePics.Where(u => u.PERS_ID.Equals(UpdateUser.PERS_ID)).ToList();
                    ObjAddress = (from a in db.Addresss
                                  where a.PERS_ID == UpdateUser.PERS_ID
                                  select a).ToList();
                    if (ObjAddress.Count > 0)
                    {
                        db.Addresss.RemoveRange(ObjAddress);
                        db.SaveChanges();
                    }
                    if (ObjPersonProfilePic.Count > 0)
                    {
                        db.PersonProfilePics.RemoveRange(ObjPersonProfilePic);
                        db.SaveChanges();
                    }


                }
                if (ObjPer != null)
                {
                    ObjPer.Social_Security_No = UpdateUser.Person.Social_Security_No;
                    ObjPer.Gender = UpdateUser.Person.Gender;
                    ObjPer.TIMETOCALL = UpdateUser.Person.TIMETOCALL;
                    ObjPer.email = UpdateUser.Person.email;

                }

                if (ObjPersonName != null)
                {
                    ObjPersonName.PERS_FNAME = UpdateUser.personname.PERS_FNAME;
                    ObjPersonName.PERS_LNAME = UpdateUser.personname.PERS_LNAME;
                    ObjPersonName.PERS_MNAME = string.IsNullOrEmpty(UpdateUser.personname.PERS_MNAME) ? "" : UpdateUser.personname.PERS_MNAME; //UpdateUser.personname.PERS_MNAME;
                    ObjPersonName.PERS_NAME_PFIX = UpdateUser.personname.PERS_NAME_PFIX;
                    ObjPersonName.PERS_NAME_SFIX = UpdateUser.personname.PERS_NAME_SFIX;
                }
                if (ObjUser != null)
                {
                    if (!string.IsNullOrEmpty(UpdateUser.OLDPASSWORD))
                    {
                        if (ObjUser.PASSWORD.Equals(UpdateUser.OLDPASSWORD))
                        {
                            ObjUser.PASSWORD = UpdateUser.PASSWORD;
                        }

                    }
                    ObjUser.ANSWER_1 = UpdateUser.ANSWER_1;
                    ObjUser.QUESTION_1 = UpdateUser.QUESTION_1;
                    ObjUser.QUESTION_2 = UpdateUser.QUESTION_2;
                    ObjUser.ANSWER_2 = UpdateUser.ANSWER_2;

                }
                using (var dbCtx = new ADPQContext())
                {

                    dbCtx.Entry(ObjPer).State = System.Data.Entity.EntityState.Modified;
                    dbCtx.Entry(ObjPersonName).State = System.Data.Entity.EntityState.Modified;
                    dbCtx.Entry(ObjUser).State = System.Data.Entity.EntityState.Modified;
                    dbCtx.SaveChanges();
                }

                foreach (var item in UpdateUser.PersonProfilePic)
                {
                    Guid Profile_FileId = Guid.NewGuid();
                    PersonProfilePic ObjPersonProfilePics = new PersonProfilePic
                    {
                        Profile_File_Id = Profile_FileId,
                        PERS_ID = UpdateUser.PERS_ID,
                        DIS_PIC_ID = item.DIS_PIC_ID,
                        Profile_File_Name = item.Profile_File_Name

                    };
                    using (var db = new ADPQContext())
                    {
                        db.PersonProfilePics.Add(ObjPersonProfilePics);
                        db.SaveChanges();
                    }
                }

                using (var db = new ADPQContext())
                {
                    foreach (var item in UpdateUser.Address)
                    {
                        Guid AddressID = Guid.NewGuid();
                        Address Add = new Address
                        {
                            ADR_ID = AddressID,
                            ADR_LINE1 = item.ADR_LINE1,
                            ADR_LINE2 = item.ADR_LINE2,
                            ADR_CITY = item.ADR_CITY,
                            ADR_STATE_CD = item.ADR_STATE_CD,
                            // ADR_CNTRY = item.ADR_CNTRY,
                            ADR_ZIP_CD = item.ADR_ZIP_CD,
                            ADR_ZIP_EXTN = item.ADR_ZIP_EXTN,
                            LOC_ID = item.LOC_ID,
                            MAILING_ADR = item.MAILING_ADR,
                            PERS_ID = UpdateUser.PERS_ID,
                            ADR_DESC = item.ADR_DESC
                        };
                        db.Addresss.Add(Add);
                        db.SaveChanges();
                        if (UpdateUser.MAILING_ADR)
                        {
                            break;
                        }

                    }


                }


                using (var dbCtx = new ADPQContext())
                {
                    ObjPhone = (from a in dbCtx.PhoneType
                                where a.PERS_ID == UpdateUser.PERS_ID
                                select a).ToList();

                    if (ObjPhone.Count > 1)
                    {
                        dbCtx.PhoneType.RemoveRange(ObjPhone);
                        dbCtx.SaveChanges();
                    }
                }

                using (var db = new ADPQContext())
                {
                    foreach (var item in UpdateUser.PhoneTypes)
                    {
                        Guid PhoneTypeId = Guid.NewGuid();
                        PhoneTypes ObjPhoneTypes = new PhoneTypes
                        {
                            PHON_TYP_ID = PhoneTypeId,
                            PHON_TYPE_CD = item.PHON_TYPE_CD,
                            PHON_TYP_DESC = item.PHON_TYP_DESC,
                            PERS_ID = UpdateUser.PERS_ID

                        };
                        db.PhoneType.Add(ObjPhoneTypes);
                        db.SaveChanges();

                    }
                }


                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public User GetUserProfile(Guid PERS_ID)
        {
            try
            {
                List<PersonProfilePic> ObjPersonProfilePic = new List<PersonProfilePic>();
                CommonRepository ObjCommon = new CommonRepository();
                User UserDetail = new User();
                using (var db = new ADPQContext())
                {
                    var res = (from u in db.Users
                               where u.PERS_ID == PERS_ID
                               select new
                               {
                                   PERS_ID = u.PERS_ID,
                                   USERNAME = u.USERNAME,
                                   QUESTION_1 = u.QUESTION_1,
                                   QUESTION_2 = u.QUESTION_2,
                                   ANSWER_1 = u.ANSWER_1,
                                   ANSWER_2 = u.ANSWER_2,
                                   personname = (from p in db.Person_Names
                                                 where p.PERS_ID.Equals(PERS_ID)
                                                 select p).FirstOrDefault(),
                                   person = (from p in db.Persons
                                             where p.PERS_ID.Equals(PERS_ID)
                                             select p).FirstOrDefault(),
                                   Address = (from a in db.Addresss
                                              where a.PERS_ID.Equals(PERS_ID)
                                              select a).ToList(),
                                   PhoneTypes = (from ph in db.PhoneType
                                                 where ph.PERS_ID.Equals(PERS_ID)
                                                 select ph).ToList(),
                                   PersonProfilePic = (from rm in db.PersonProfilePics
                                                       where rm.PERS_ID.Equals(PERS_ID)
                                                       select rm).ToList(),



                               }).AsEnumerable().Select(x => new User
                               {
                                   PERS_ID = x.PERS_ID,
                                   USERNAME = x.USERNAME,
                                   QUESTION_1 = x.QUESTION_1,
                                   QUESTION_2 = x.QUESTION_2,
                                   ANSWER_1 = x.ANSWER_1,
                                   ANSWER_2 = x.ANSWER_2,
                                   personname = x.personname,
                                   Person = x.person,
                                   Address = x.Address,
                                   PhoneTypes = x.PhoneTypes,
                                   PersonProfilePic = x.PersonProfilePic

                               }).ToList();
                    UserDetail = res.FirstOrDefault();
                    UserDetail.MAILING_ADR = (UserDetail.Address.Count > 1) ? false : true;

                    List<ProfilePic> ImageList = new List<ProfilePic>();

                    foreach (var item in UserDetail.PersonProfilePic)
                    {
                        string MediaType = (item.DIS_PIC_ID.Split('.')[1].ToLower().Equals("png")) ? "image/png" : (item.DIS_PIC_ID.Split('.')[1].ToLower().Equals("image/jpg")) ? "image/jpg" : "image/jpeg";
                        ProfilePic ab = new ProfilePic
                        {
                            Image = ObjCommon.getImage(item.PERS_ID, item.DIS_PIC_ID, MediaType),
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
                                               PERS_ID = a.PERS_ID,
                                               DIS_PIC_ID = a.DIS_PIC_ID,
                                               Profile_File_Name = a.Profile_File_Name,
                                               Image = b.Image

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
                // }
                return UserDetail;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ValidateToken(Guid AuthToken)
        {
            using (var db = new ADPQContext())
            {
                Token TokenExists = db.Tokens.FirstOrDefault(u => u.TokenId == AuthToken && u.IsActive == true);

                var cutoff = DateTime.Now.AddMinutes(-20);

                if (TokenExists == null)
                {
                    //  CreateToken(AuthToken.Pers_Id);
                    return false;
                }
                else
                {
                    bool asd = TokenExists.LastTokenCheck >= cutoff;

                    if (TokenExists.LastTokenCheck >= cutoff)
                    {
                        TokenExists.LastTokenCheck = DateTime.Now;

                        db.Entry(TokenExists).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        TokenExists.IsActive = false;
                        db.Entry(TokenExists).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return false;
                    }
                }
            }
        }

        public Guid CreateToken(Guid PERS_ID)
        {
            using (var db = new ADPQContext())
            {
                Guid NewToken = Guid.NewGuid();
                Token objToken = new Token
                {
                    Pers_Id = PERS_ID,
                    TokenId = NewToken,
                    CreatedTime = DateTime.Now,
                    LastTokenCheck = DateTime.Now,
                    IsActive = true

                };
                db.Tokens.Add(objToken);
                db.SaveChanges();
                return NewToken;
            }
        }

        public Guid GetPER_ID(Guid Token)
        {
            using (var db = new ADPQContext())
            {
                return db.Tokens.Where(u => u.TokenId == Token).Select(x => x.Pers_Id).FirstOrDefault();
            }
        }

        public User login(User p, out Guid Token)
        {
            throw new NotImplementedException();
        }

        public bool ValidateToken(Token AuthToken)
        {
            throw new NotImplementedException();
        }

        public bool CheckUserID(string Username, int type)
        {
            using (var dbCtx = new ADPQContext())
            {
                if (type==0)
                return dbCtx.Users.Any(u => u.USERNAME.Equals(Username));
            else
                return dbCtx.Persons.Any(u => u.email.Equals(Username));
            }
        }

        public bool MoveTempFile(string uploadPath, string SourceFilePath, string TagertFilePath)
        {
            if (!System.IO.Directory.Exists(uploadPath))
                System.IO.Directory.CreateDirectory(uploadPath);

            System.IO.File.Move(SourceFilePath, TagertFilePath);
            return true;

        }
    }
}
