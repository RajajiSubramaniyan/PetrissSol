using System;
using System.Linq;
using Petriss.Models.DB;
using System.Collections.Generic;
using Petriss.Models.ViewModel;

namespace Petriss.Models.EntityManager
{
    public class OrganizationManager
    {
        public void AddUserAccount(UserSignUpView user)
        {

            using (PetrissDbEntities db = new PetrissDbEntities())
            {

                PSYSUser SU = new PSYSUser();
                SU.LoginName = user.LoginName;
                SU.PasswordEncryptedText = user.Password;
                SU.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                SU.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1; ;
                SU.RowCreatedDateTime = DateTime.Now;
                SU.RowMOdifiedDateTime = DateTime.Now;

                db.PSYSUsers.Add(SU);
                db.SaveChanges();

                PSYSUserProfile SUP = new PSYSUserProfile();
                SUP.SYSUserID = SU.SYSUserID;
                SUP.FirstName = user.FirstName;
                SUP.LastName = user.LastName;
                SUP.Gender = user.Gender;
                SUP.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                SUP.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                SUP.RowCreatedDateTime = DateTime.Now;
                SUP.RowModifiedDateTime = DateTime.Now;

                db.PSYSUserProfiles.Add(SUP);
                db.SaveChanges();


                if (user.LOOKUPRoleID > 0)
                {
                    PSYSUserRole SUR = new PSYSUserRole();
                    SUR.LOOKUPRoleID = user.LOOKUPRoleID;
                    SUR.SYSUserID = user.SYSUserID;
                    SUR.IsActive = true;
                    SUR.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                    SUR.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                    SUR.RowCreatedDateTime = DateTime.Now;
                    SUR.RowModifiedDateTime = DateTime.Now;

                    db.PSYSUserRoles.Add(SUR);
                    db.SaveChanges();
                }
            }
        }

        public void UpdateUserAccount(UserProfileView user)
        {

            using (PetrissDbEntities db = new PetrissDbEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {

                        PSYSUser SU = db.PSYSUsers.Find(user.SYSUserID);
                        SU.LoginName = user.LoginName;
                        SU.PasswordEncryptedText = user.Password;
                        SU.RowCreatedSYSUserID = user.SYSUserID;
                        SU.RowModifiedSYSUserID = user.SYSUserID;
                        SU.RowCreatedDateTime = DateTime.Now;
                        SU.RowMOdifiedDateTime = DateTime.Now;

                        db.SaveChanges();

                        var userProfile = db.PSYSUserProfiles.Where(o => o.SYSUserID == user.SYSUserID);
                        if (userProfile.Any())
                        {
                            PSYSUserProfile SUP = userProfile.FirstOrDefault();
                            SUP.SYSUserID = SU.SYSUserID;
                            SUP.FirstName = user.FirstName;
                            SUP.LastName = user.LastName;
                            SUP.Gender = user.Gender;
                            SUP.RowCreatedSYSUserID = user.SYSUserID;
                            SUP.RowModifiedSYSUserID = user.SYSUserID;
                            SUP.RowCreatedDateTime = DateTime.Now;
                            SUP.RowModifiedDateTime = DateTime.Now;

                            db.SaveChanges();
                        }

                        if (user.LOOKUPRoleID > 0)
                        {
                            var userRole = db.PSYSUserRoles.Where(o => o.SYSUserID == user.SYSUserID);
                            PSYSUserRole SUR = null;
                            if (userRole.Any())
                            {
                                SUR = userRole.FirstOrDefault();
                                SUR.LOOKUPRoleID = user.LOOKUPRoleID;
                                SUR.SYSUserID = user.SYSUserID;
                                SUR.IsActive = true;
                                SUR.RowCreatedSYSUserID = user.SYSUserID;
                                SUR.RowModifiedSYSUserID = user.SYSUserID;
                                SUR.RowCreatedDateTime = DateTime.Now;
                                SUR.RowModifiedDateTime = DateTime.Now;
                            }
                            else
                            {
                                SUR = new PSYSUserRole();
                                SUR.LOOKUPRoleID = user.LOOKUPRoleID;
                                SUR.SYSUserID = user.SYSUserID;
                                SUR.IsActive = true;
                                SUR.RowCreatedSYSUserID = user.SYSUserID;
                                SUR.RowModifiedSYSUserID = user.SYSUserID;
                                SUR.RowCreatedDateTime = DateTime.Now;
                                SUR.RowModifiedDateTime = DateTime.Now;
                                db.PSYSUserRoles.Add(SUR);
                            }

                            db.SaveChanges();
                        }
                        dbContextTransaction.Commit();
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }
      

        public UserProfileView GetUserProfile(int userID)
        {
            UserProfileView UPV = new UserProfileView();
            using (PetrissDbEntities db = new PetrissDbEntities())
            {
                var user = db.PSYSUsers.Find(userID);
                if (user != null)
                {
                    UPV.SYSUserID = user.SYSUserID;
                    UPV.LoginName = user.LoginName;
                    UPV.Password = user.PasswordEncryptedText;

                    var SUP = db.PSYSUserProfiles.Find(userID);
                    if (SUP != null)
                    {
                        UPV.FirstName = SUP.FirstName;
                        UPV.LastName = SUP.LastName;
                        UPV.Gender = SUP.Gender;
                    }

                    var SUR = db.PSYSUserRoles.Find(userID);
                    if (SUR != null)
                    {
                        UPV.LOOKUPRoleID = SUR.LOOKUPRoleID;
                        UPV.RoleName = SUR.PLOOKUPRole.RoleName;
                        UPV.IsRoleActive = SUR.IsActive;
                    }
                }
            }
            return UPV;
        }

        public void DeleteUser(int userID)
        {
            using (PetrissDbEntities db = new PetrissDbEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {

                        var SUR = db.PSYSUserRoles.Where(o => o.SYSUserID == userID);
                        if (SUR.Any())
                        {
                            db.PSYSUserRoles.Remove(SUR.FirstOrDefault());
                            db.SaveChanges();
                        }

                        var SUP = db.PSYSUserProfiles.Where(o => o.SYSUserID == userID);
                        if (SUP.Any())
                        {
                            db.PSYSUserProfiles.Remove(SUP.FirstOrDefault());
                            db.SaveChanges();
                        }

                        var SU = db.PSYSUsers.Where(o => o.SYSUserID == userID);
                        if (SU.Any())
                        {
                            db.PSYSUsers.Remove(SU.FirstOrDefault());
                            db.SaveChanges();
                        }

                        dbContextTransaction.Commit();
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }
    }
}