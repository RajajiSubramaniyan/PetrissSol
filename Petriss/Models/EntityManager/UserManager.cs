using System;
using System.Linq;
using Petriss.Models.DB;
using System.Collections.Generic;
using Petriss.Models.ViewModel;
using System.Web;
using Petriss.Utilities;

namespace Petriss.Models.EntityManager
{
    public class UserManager
    {
       
     
        public void AddUserAccount(UserSignUpView user)
        {
            string baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            using (PetrissEntities db = new PetrissEntities())
            {

                User _user = new User();
                _user.EmailId = user.EmailAddress;

                var keyNew = Helper.GeneratePassword(10);
                var password = Helper.EncodePassword(_user.Password, keyNew);
                _user.Password = password;
                _user.Password = user.Password;
                _user.UserActivationLink = baseUrl + Guid.NewGuid();
                _user.CreatedByUserId = user.UserId > 0 ? user.UserId : 1;
                _user.ModifiedByUserId = user.UserId > 0 ? user.UserId : 1; ;
                _user.CreatedDateTime = DateTime.Now;
                _user.ModifiedDateTime = DateTime.Now;
                db.Users.Add(_user);
                db.SaveChanges();

                UsersProfile _userprofiles = new UsersProfile();
                _userprofiles.UserId = _user.UserId;
                _userprofiles.PreferredName = user.PrefferedName;
                _userprofiles.FirstName = user.FirstName;
                _userprofiles.LastName = user.LastName;
                _userprofiles.CreatedByUserId = user.UserId > 0 ? user.UserId : 1;
                _userprofiles.ModifiedByUserId = user.UserId > 0 ? user.UserId : 1;
                _userprofiles.CreatedDateTime = DateTime.Now;
                _userprofiles.ModifiedDateTime = DateTime.Now;
                db.UsersProfiles.Add(_userprofiles);
                db.SaveChanges();


                if (user.UserLookupRoleId > 0)
                {
                    UsersRole _usersrole = new UsersRole();
                    _usersrole.UserLookupRoleId = user.UserLookupRoleId;
                    _usersrole.UserId = user.UserId;
                    _usersrole.IsActive = true;
                    _usersrole.CreatedByUserId = user.UserId > 0 ? user.UserId : 1;
                    _usersrole.ModifiedByUserId = user.UserId > 0 ? user.UserId : 1;
                    _usersrole.CreatedDateTime = DateTime.Now;
                    _usersrole.ModifiedDateTime = DateTime.Now;

                    db.UsersRoles.Add(_usersrole);
                    db.SaveChanges();
                }
            }
        }

        public void UpdateUserAccount(UserProfileView user)
        {

            using (PetrissEntities db = new PetrissEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {

                        User _user = db.Users.Find(user.UserId);
                        _user.EmailId = user.EmailAddress;
                        _user.Password = user.Password;
                        _user.CreatedByUserId = user.UserId;
                        _user.ModifiedByUserId = user.UserId;
                        _user.CreatedDateTime = DateTime.Now;
                        _user.ModifiedDateTime = DateTime.Now;

                        db.SaveChanges();

                        var userProfile = db.UsersProfiles.Where(o => o.UserId == user.UserId);
                        if (userProfile.Any())
                        {
                            UsersProfile _userprofile = userProfile.FirstOrDefault();
                            _userprofile.UserId = _user.UserId;
                            _userprofile.FirstName = user.FirstName;
                            _userprofile.LastName = user.LastName;
                            _userprofile.PhoneNumber = user.PhoneNumber;
                            _userprofile.CreatedByUserId = user.UserId;
                            _userprofile.ModifiedByUserId = user.UserId;
                            _userprofile.CreatedDateTime = DateTime.Now;
                            _userprofile.ModifiedDateTime = DateTime.Now;

                            db.SaveChanges();
                        }

                        if (user.UserLookupRoleId > 0)
                        {
                            var userRole = db.UsersRoles.Where(o => o.UserId == user.UserId);
                            UsersRole SUR = null;
                            if (userRole.Any())
                            {
                                SUR = userRole.FirstOrDefault();
                                SUR.UserLookupRoleId = user.UserLookupRoleId;
                                SUR.UserId = user.UserId;
                                SUR.IsActive = true;
                                SUR.CreatedByUserId = user.UserId;
                                SUR.ModifiedByUserId = user.UserId;
                                SUR.CreatedDateTime = DateTime.Now;
                                SUR.ModifiedDateTime = DateTime.Now;
                            }
                            else
                            {
                                SUR = new UsersRole();
                                SUR.UserLookupRoleId = user.UserLookupRoleId;
                                SUR.UserId = user.UserId;
                                SUR.IsActive = true;
                                SUR.CreatedByUserId = user.UserId;
                                SUR.ModifiedByUserId = user.UserId;
                                SUR.CreatedDateTime = DateTime.Now;
                                SUR.ModifiedDateTime = DateTime.Now;
                                db.UsersRoles.Add(SUR);
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
        public bool IsLoginNameExist(string loginName)
        {
            using (PetrissEntities db = new PetrissEntities())
            {
                return db.Users.Where(o => o.EmailId.Equals(loginName)).Any();
            }
        }

        public string GetUserPassword(string loginName)
        {
            using (PetrissEntities db = new PetrissEntities())
            {
                var user = db.Users.Where(o => o.EmailId.ToLower().Equals(loginName));
                if (user.Any())
                    return user.FirstOrDefault().Password;
                else
                    return string.Empty;
            }
        }
        public string GetUserActivationLink(string loginName)
        {
            using (PetrissEntities db = new PetrissEntities())
            {
                var user = db.Users.Where(o => o.EmailId.ToLower().Equals(loginName));
                if (user.Any())
                    return user.FirstOrDefault().UserActivationLink;
                else
                    return string.Empty;
            }
        }
        public string[] GetRolesForUser(string loginName)
        {
            using (PetrissEntities db = new PetrissEntities())
            {
                User _user = db.Users.Where(o => o.EmailId.ToLower().Equals(loginName))?.FirstOrDefault();
                if (_user != null)
                {
                    var roles = from q in db.UsersRoles
                                join r in db.UserLookupRoles on q.UserLookupRoleId equals r.UserLookupRoleId
                                select r.RoleName;

                    if (roles != null)
                    {
                        return roles.ToArray();
                    }
                }

                return new string[] { };
            }
        }
        public bool IsUserInRole(string loginName, string roleName)
        {
            using (PetrissEntities db = new PetrissEntities())
            {
                User _user = db.Users.Where(o => o.EmailId.ToLower().Equals(loginName))?.FirstOrDefault();
                if (_user != null)
                {
                    var roles = from q in db.UsersRoles
                                join r in db.UserLookupRoles on q.UserLookupRoleId equals r.UserLookupRoleId
                                where r.RoleName.Equals(roleName) && q.UserId.Equals(_user.UserId)
                                select r.RoleName;

                    if (roles != null)
                    {
                        return roles.Any();
                    }
                }

                return false;
            }
        }

        public List<UserLookupAvailableRole> GetAllRoles()
        {
            using (PetrissEntities db = new PetrissEntities())
            {
                var roles = db.UserLookupRoles.Select(o => new UserLookupAvailableRole
                {
                    UserLookupRoleId = o.UserLookupRoleId,
                    RoleName = o.RoleName,
                    RoleDescription = o.RoleDescription
                }).ToList();

                return roles;
            }
        }

        public int GetUserID(string loginName)
        {
            using (PetrissEntities db = new PetrissEntities())
            {
                var user = db.Users.Where(o => o.EmailId.Equals(loginName));
                if (user.Any())
                    return user.FirstOrDefault().UserId;
            }
            return 0;
        }
        public List<UserProfileView> GetAllUserProfiles()
        {
            List<UserProfileView> profiles = new List<UserProfileView>();
            using (PetrissEntities db = new PetrissEntities())
            {
                UserProfileView UPV;
                var users = db.Users.ToList();

                foreach (User u in db.Users)
                {
                    UPV = new UserProfileView();
                    UPV.UserId = u.UserId;
                    UPV.EmailAddress = u.EmailId;
                    UPV.Password = u.Password;

                    var _userprofile = db.UsersProfiles.Find(u.UserId);
                    if (_userprofile != null)
                    {
                        UPV.FirstName = _userprofile.FirstName;
                        UPV.LastName = _userprofile.LastName;
                        UPV.PhoneNumber = _userprofile.PhoneNumber;
                    }

                    var SUR = db.UsersRoles.Where(o => o.UserId.Equals(u.UserId));
                    if (SUR.Any())
                    {
                        var userRole = SUR.FirstOrDefault();
                        UPV.UserLookupRoleId = userRole.UserLookupRoleId;
                        UPV.RoleName = userRole.UserLookupRole.RoleName;
                        UPV.IsRoleActive = userRole.IsActive;
                    }

                    profiles.Add(UPV);
                }
            }

            return profiles;
        }

        public UserDataView GetUserDataView(string loginName)
        {
            UserDataView UDV = new UserDataView();
            List<UserProfileView> profiles = GetAllUserProfiles();
            List<UserLookupAvailableRole> roles = GetAllRoles();

            int? userAssignedRoleID = 0, userID = 0;
            string userGender = string.Empty;

            userID = GetUserID(loginName);
            using (PetrissEntities db = new PetrissEntities())
            {
                userAssignedRoleID = db.UsersRoles.Where(o => o.UserId == userID)?.FirstOrDefault().UserLookupRoleId;
                userGender = db.UsersProfiles.Where(o => o.UserId == userID)?.FirstOrDefault().Gender;
            }

            List<Gender> genders = new List<Gender>();
            genders.Add(new Gender { Text = "Male", Value = "M" });
            genders.Add(new Gender { Text = "Female", Value = "F" });

            UDV.UserProfile = profiles;
            UDV.UserRoles = new UserRoles { SelectedRoleId = userAssignedRoleID, UserRoleList = roles };
            UDV.UserGender = new UserGender { SelectedGender = userGender, Gender = genders };
            return UDV;
        }

        public UserProfileView GetUserProfile(int userID)
        {
            UserProfileView UPV = new UserProfileView();
            using (PetrissEntities db = new PetrissEntities())
            {
                var user = db.Users.Find(userID);
                if (user != null)
                {
                    UPV.UserId = user.UserId;
                    UPV.EmailAddress = user.EmailId;
                    UPV.Password = user.Password;

                    var _userprofile = db.UsersProfiles.Find(userID);
                    if (_userprofile != null)
                    {
                        UPV.FirstName = _userprofile.FirstName;
                        UPV.LastName = _userprofile.LastName;
                        UPV.PhoneNumber = _userprofile.PhoneNumber;
                    }

                    var SUR = db.UsersRoles.Find(userID);
                    if (SUR != null)
                    {
                        UPV.UserLookupRoleId = SUR.UserLookupRoleId;
                        UPV.RoleName = SUR.UserLookupRole.RoleName;
                        UPV.IsRoleActive = SUR.IsActive;
                    }
                }
            }
            return UPV;
        }

        public void DeleteUser(int userID)
        {
            using (PetrissEntities db = new PetrissEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {

                        var SUR = db.UsersRoles.Where(o => o.UserId == userID);
                        if (SUR.Any())
                        {
                            db.UsersRoles.Remove(SUR.FirstOrDefault());
                            db.SaveChanges();
                        }

                        var _userprofile = db.UsersProfiles.Where(o => o.UserId == userID);
                        if (_userprofile.Any())
                        {
                            db.UsersProfiles.Remove(_userprofile.FirstOrDefault());
                            db.SaveChanges();
                        }

                        var _user = db.Users.Where(o => o.UserId == userID);
                        if (_user.Any())
                        {
                            db.Users.Remove(_user.FirstOrDefault());
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