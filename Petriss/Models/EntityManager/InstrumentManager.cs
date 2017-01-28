using System;
using System.Linq;
using Petriss.Models.DB;
using System.Collections.Generic;
using Petriss.Models.ViewModel;

namespace Petriss.Models.EntityManager
{
    /// <summary>
    /// 
    /// </summary>
    public class InstrumentManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public void AddInstrument(UserSignUpView user)
        {

            using (PetrissEntities db = new PetrissEntities())
            {

                User _user = new User();
                _user.EmailId = user.EmailAddress;
                _user.Password = user.Password;
                _user.CreatedByUserId = user.UserId > 0 ? user.UserId : 1;
                _user.ModifiedByUserId = user.UserId > 0 ? user.UserId : 1; ;
                _user.CreatedDateTime = DateTime.Now;
                _user.ModifiedDateTime = DateTime.Now;
                db.Users.Add(_user);
                db.SaveChanges();

                UsersProfile _userprofiles = new UsersProfile();
                _userprofiles.UserId = _user.UserId;
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
        /// <summary>
        /// Update Instrument
        /// </summary>
        /// <param name="user"></param>
        public void UpdateInstrument(UserProfileView user)
        {

            using (PetrissEntities db = new PetrissEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {

                        User _user = db.Users.Find(user.UserId);
                        _user.EmailId = user.LoginName;
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
                            _userprofile.Gender = user.Gender;
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


        public UserProfileView GetInstrumentById(int userID)
        {
            UserProfileView _userprofile = new UserProfileView();
            using (PetrissEntities db = new PetrissEntities())
            {
                var user = db.Users.Find(userID);
                if (user != null)
                {
                    _userprofile.UserId = user.UserId;
                    _userprofile.LoginName = user.EmailId;
                    _userprofile.Password = user.Password;

                    var SUP = db.UsersProfiles.Find(userID);
                    if (SUP != null)
                    {
                        _userprofile.FirstName = SUP.FirstName;
                        _userprofile.LastName = SUP.LastName;
                        _userprofile.Gender = SUP.Gender;
                    }

                    var SUR = db.UsersRoles.Find(userID);
                    if (SUR != null)
                    {
                        _userprofile.UserLookupRoleId = SUR.UserLookupRoleId;
                        _userprofile.RoleName = SUR.UserLookupRole.RoleName;
                        _userprofile.IsRoleActive = SUR.IsActive;
                    }
                }
            }
            return _userprofile;
        }

        public void DeleteInstrument(int userID)
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

                        var SUP = db.UsersProfiles.Where(o => o.UserId == userID);
                        if (SUP.Any())
                        {
                            db.UsersProfiles.Remove(SUP.FirstOrDefault());
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