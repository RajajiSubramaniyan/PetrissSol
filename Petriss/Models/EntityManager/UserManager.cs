using System;
using System.Linq;
using Petriss.Models.DB;
using System.Collections.Generic;
using Petriss.Models.ViewModel;

namespace Petriss.Models.EntityManager
{
    public class UserManager
    {
        public void AddUserAccount(UserSignUpView user)
        {

            using (petdevEntities db = new petdevEntities())
            {
                PETUser SUP = new PETUser();
                SUP.FirstName = user.FirstName;
                SUP.LastName = user.LastName;
                SUP.PrefferedName = user.PrefferedName;
                SUP.TermsandConditionsStatus = false;
                SUP.PasswordEncryptedText = user.Password; // TODO get encrypted
                SUP.EmailAddress = user.EmailAddress;
                SUP.RowCreatedDateTime = DateTime.Now;
                SUP.RowModifiedDateTime = DateTime.Now;

                db.PETUsers.Add(SUP);
                db.SaveChanges();

            }
        }

        //public void UpdateUserAccount(UserProfileView user)
        //{

        //    using (PetrissDbEntities db = new PetrissDbEntities())
        //    {
        //        using (var dbContextTransaction = db.Database.BeginTransaction())
        //        {
        //            try
        //            {

        //                PSYSUser SU = db.PSYSUsers.Find(user.SYSUserID);
        //                SU.LoginName = user.LoginName;
        //                SU.PasswordEncryptedText = user.Password;
        //                SU.RowCreatedSYSUserID = user.SYSUserID;
        //                SU.RowModifiedSYSUserID = user.SYSUserID;
        //                SU.RowCreatedDateTime = DateTime.Now;
        //                SU.RowMOdifiedDateTime = DateTime.Now;

        //                db.SaveChanges();

        //                var userProfile = db.PSYSUserProfiles.Where(o => o.SYSUserID == user.SYSUserID);
        //                if (userProfile.Any())
        //                {
        //                    PSYSUserProfile SUP = userProfile.FirstOrDefault();
        //                    SUP.SYSUserID = SU.SYSUserID;
        //                    SUP.FirstName = user.FirstName;
        //                    SUP.LastName = user.LastName;
        //                    SUP.Gender = user.Gender;
        //                    SUP.RowCreatedSYSUserID = user.SYSUserID;
        //                    SUP.RowModifiedSYSUserID = user.SYSUserID;
        //                    SUP.RowCreatedDateTime = DateTime.Now;
        //                    SUP.RowModifiedDateTime = DateTime.Now;

        //                    db.SaveChanges();
        //                }

        //                if (user.LOOKUPRoleID > 0)
        //                {
        //                    var userRole = db.PSYSUserRoles.Where(o => o.SYSUserID == user.SYSUserID);
        //                    PSYSUserRole SUR = null;
        //                    if (userRole.Any())
        //                    {
        //                        SUR = userRole.FirstOrDefault();
        //                        SUR.LOOKUPRoleID = user.LOOKUPRoleID;
        //                        SUR.SYSUserID = user.SYSUserID;
        //                        SUR.IsActive = true;
        //                        SUR.RowCreatedSYSUserID = user.SYSUserID;
        //                        SUR.RowModifiedSYSUserID = user.SYSUserID;
        //                        SUR.RowCreatedDateTime = DateTime.Now;
        //                        SUR.RowModifiedDateTime = DateTime.Now;
        //                    }
        //                    else
        //                    {
        //                        SUR = new PSYSUserRole();
        //                        SUR.LOOKUPRoleID = user.LOOKUPRoleID;
        //                        SUR.SYSUserID = user.SYSUserID;
        //                        SUR.IsActive = true;
        //                        SUR.RowCreatedSYSUserID = user.SYSUserID;
        //                        SUR.RowModifiedSYSUserID = user.SYSUserID;
        //                        SUR.RowCreatedDateTime = DateTime.Now;
        //                        SUR.RowModifiedDateTime = DateTime.Now;
        //                        db.PSYSUserRoles.Add(SUR);
        //                    }

        //                    db.SaveChanges();
        //                }
        //                dbContextTransaction.Commit();
        //            }
        //            catch
        //            {
        //                dbContextTransaction.Rollback();
        //            }
        //        }
        //    }
        //}
        public bool IsLoginNameExist(string loginName)
        {
            using (petdevEntities db = new petdevEntities())
            {
                return false;// db.PETUsers.Where(o => o.EmailAddress.Equals(loginName)).Any();
            }
        }

        public string GetUserPassword(string loginName)
        {
            using (petdevEntities db = new petdevEntities())
            {
                var user = db.PETUsers.Where(o => o.EmailAddress.ToLower().Equals(loginName));
                if (user.Any())
                    return user.FirstOrDefault().PasswordEncryptedText;
                else
                    return string.Empty;
            }
        }

        //public string[] GetRolesForUser(string loginName)
        //{
        //    using (PetrissDbEntities db = new PetrissDbEntities())
        //    {
        //        PSYSUser SU = db.PSYSUsers.Where(o => o.LoginName.ToLower().Equals(loginName))?.FirstOrDefault();
        //        if (SU != null)
        //        {
        //            var roles = from q in db.PSYSUserRoles
        //                        join r in db.PLOOKUPRoles on q.LOOKUPRoleID equals r.LOOKUPRoleID
        //                        select r.RoleName;

        //            if (roles != null)
        //            {
        //                return roles.ToArray();
        //            }
        //        }

        //        return new string[] { };
        //    }
        //}
        //public bool IsUserInRole(string loginName, string roleName)
        //{
        //    using (PetrissDbEntities db = new PetrissDbEntities())
        //    {
        //        PSYSUser SU = db.PSYSUsers.Where(o => o.LoginName.ToLower().Equals(loginName))?.FirstOrDefault();
        //        if (SU != null)
        //        {
        //            var roles = from q in db.PSYSUserRoles
        //                        join r in db.PLOOKUPRoles on q.LOOKUPRoleID equals r.LOOKUPRoleID
        //                        where r.RoleName.Equals(roleName) && q.SYSUserID.Equals(SU.SYSUserID)
        //                        select r.RoleName;

        //            if (roles != null)
        //            {
        //                return roles.Any();
        //            }
        //        }

        //        return false;
        //    }
        //}

        //public List<LOOKUPAvailableRole> GetAllRoles()
        //{
        //    using (PetrissDbEntities db = new PetrissDbEntities())
        //    {
        //        var roles = db.PLOOKUPRoles.Select(o => new LOOKUPAvailableRole
        //        {
        //            LOOKUPRoleID = o.LOOKUPRoleID,
        //            RoleName = o.RoleName,
        //            RoleDescription = o.RoleDescription
        //        }).ToList();

        //        return roles;
        //    }
        //}

        public int GetUserID(string loginName)
        {
            using (petdevEntities db = new petdevEntities())
            {
                var user = db.PETUsers.Where(o => o.EmailAddress.Equals(loginName));
                if (user.Any())
                    return user.FirstOrDefault().UserProfileID;
            }
            return 0;
        }
        //public List<UserProfileView> GetAllUserProfiles()
        //{
        //    List<UserProfileView> profiles = new List<UserProfileView>();
        //    using (PetrissDbEntities db = new PetrissDbEntities())
        //    {
        //        UserProfileView UPV;
        //        var users = db.PSYSUsers.ToList();

        //        foreach (PSYSUser u in db.PSYSUsers)
        //        {
        //            UPV = new UserProfileView();
        //            UPV.SYSUserID = u.SYSUserID;
        //            UPV.LoginName = u.LoginName;
        //            UPV.Password = u.PasswordEncryptedText;

        //            var SUP = db.PSYSUserProfiles.Find(u.SYSUserID);
        //            if (SUP != null)
        //            {
        //                UPV.FirstName = SUP.FirstName;
        //                UPV.LastName = SUP.LastName;
        //                UPV.Gender = SUP.Gender;
        //            }

        //            var SUR = db.PSYSUserRoles.Where(o => o.SYSUserID.Equals(u.SYSUserID));
        //            if (SUR.Any())
        //            {
        //                var userRole = SUR.FirstOrDefault();
        //                UPV.LOOKUPRoleID = userRole.LOOKUPRoleID;
        //                UPV.RoleName = userRole.PLOOKUPRole.RoleName;
        //                UPV.IsRoleActive = userRole.IsActive;
        //            }

        //            profiles.Add(UPV);
        //        }
        //    }

        //    return profiles;
        //}

        //public UserDataView GetUserDataView(string loginName)
        //{
        //    UserDataView UDV = new UserDataView();
        //    List<UserProfileView> profiles = GetAllUserProfiles();
        //    List<LOOKUPAvailableRole> roles = GetAllRoles();

        //    int? userAssignedRoleID = 0, userID = 0;
        //    string userGender = string.Empty;

        //    userID = GetUserID(loginName);
        //    using (PetrissDbEntities db = new PetrissDbEntities())
        //    {
        //        userAssignedRoleID = db.PSYSUserRoles.Where(o => o.SYSUserID == userID)?.FirstOrDefault().LOOKUPRoleID;
        //        userGender = db.PSYSUserProfiles.Where(o => o.SYSUserID == userID)?.FirstOrDefault().Gender;
        //    }

        //    List<Gender> genders = new List<Gender>();
        //    genders.Add(new Gender { Text = "Male", Value = "M" });
        //    genders.Add(new Gender { Text = "Female", Value = "F" });

        //    UDV.UserProfile = profiles;
        //    UDV.UserRoles = new UserRoles { SelectedRoleID = userAssignedRoleID, UserRoleList = roles };
        //    UDV.UserGender = new UserGender { SelectedGender = userGender, Gender = genders };
        //    return UDV;
        //}

        //public UserProfileView GetUserProfile(int userID)
        //{
        //    UserProfileView UPV = new UserProfileView();
        //    using (PetrissDbEntities db = new PetrissDbEntities())
        //    {
        //        var user = db.PSYSUsers.Find(userID);
        //        if (user != null)
        //        {
        //            UPV.SYSUserID = user.SYSUserID;
        //            UPV.LoginName = user.LoginName;
        //            UPV.Password = user.PasswordEncryptedText;

        //            var SUP = db.PSYSUserProfiles.Find(userID);
        //            if (SUP != null)
        //            {
        //                UPV.FirstName = SUP.FirstName;
        //                UPV.LastName = SUP.LastName;
        //                UPV.Gender = SUP.Gender;
        //            }

        //            var SUR = db.PSYSUserRoles.Find(userID);
        //            if (SUR != null)
        //            {
        //                UPV.LOOKUPRoleID = SUR.LOOKUPRoleID;
        //                UPV.RoleName = SUR.PLOOKUPRole.RoleName;
        //                UPV.IsRoleActive = SUR.IsActive;
        //            }
        //        }
        //    }
        //    return UPV;
        //}

        //public void DeleteUser(int userID)
        //{
        //    using (PetrissDbEntities db = new PetrissDbEntities())
        //    {
        //        using (var dbContextTransaction = db.Database.BeginTransaction())
        //        {
        //            try
        //            {

        //                var SUR = db.PSYSUserRoles.Where(o => o.SYSUserID == userID);
        //                if (SUR.Any())
        //                {
        //                    db.PSYSUserRoles.Remove(SUR.FirstOrDefault());
        //                    db.SaveChanges();
        //                }

        //                var SUP = db.PSYSUserProfiles.Where(o => o.SYSUserID == userID);
        //                if (SUP.Any())
        //                {
        //                    db.PSYSUserProfiles.Remove(SUP.FirstOrDefault());
        //                    db.SaveChanges();
        //                }

        //                var SU = db.PSYSUsers.Where(o => o.SYSUserID == userID);
        //                if (SU.Any())
        //                {
        //                    db.PSYSUsers.Remove(SU.FirstOrDefault());
        //                    db.SaveChanges();
        //                }

        //                dbContextTransaction.Commit();
        //            }
        //            catch
        //            {
        //                dbContextTransaction.Rollback();
        //            }
        //        }
        //    }
        //}
    }
}