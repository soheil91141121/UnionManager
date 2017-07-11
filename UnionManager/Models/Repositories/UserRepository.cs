using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnionManager.Models.Repositories
{
    public class UserRepository : IDisposable
    {
        private UnionManager.Models.DomainModels.db_UnionEntities db = null;

        public UserRepository()
        {
            db = new DomainModels.db_UnionEntities();
        }

        public bool Exist(string username, string password)
        {
            try
            {
                return db.Users.Where(p => p.Username == username && p.Password == password).Any();
            }
            catch
            {
                return false;
            }
        }

        public bool Add(UnionManager.Models.DomainModels.User entity, out string message, bool autoSave = true)
        {
            try
            {
                db.Users.Add(entity);
                if (autoSave)
                {
                    bool result = Convert.ToBoolean(db.SaveChanges());
                    message = "";
                    return result;
                }
                else
                {
                    message = "";
                    return false;
                }
            }
            catch(Exception ex)
            {
                message = ex.GetBaseException().Message;
                return false;
            }
        }

        public bool Update(UnionManager.Models.DomainModels.User entity, string fileName, string oldImage, out string message, bool autoSave = true)
        {
            try
            {
                if (fileName != null)
                    entity.Image = fileName;
                else
                    if (oldImage != null)
                        entity.Image = oldImage;

                UnionManager.Models.DomainModels.User user = Find(entity.Id);
                db.Entry(user).State = System.Data.Entity.EntityState.Detached;
                db.Users.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                if (autoSave)
                {
                    message = "";
                    return Convert.ToBoolean(db.SaveChanges());
                }
                else
                {
                    message = "";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = ex.GetBaseException().Message;
                return false;
            }
        }

        public bool Delete(UnionManager.Models.DomainModels.User entity, out string message, bool autoSave = true)
        {
            try
            {
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                if (autoSave)
                {
                    bool result = Convert.ToBoolean(db.SaveChanges());
                    message = "";
                    return result;
                }
                else
                {
                    message = "";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = ex.GetBaseException().Message;
                return false;
            }
        }

        public bool Delete(long id, out string message, bool autoSave = true)
        {
            try
            {
                var entity = db.Users.Find(id);
                db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                if (autoSave)
                {
                    bool result = Convert.ToBoolean(db.SaveChanges());
                    message = "";
                    return result;
                }
                else
                {
                    message = "";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = ex.GetBaseException().Message;
                return false;
            }
        }

        public UnionManager.Models.DomainModels.User Find(long id)
        {
            try
            {
                return db.Users.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<UnionManager.Models.DomainModels.User> Where(System.Linq.Expressions.Expression<Func<UnionManager.Models.DomainModels.User, bool>> Predicate)
        {
            try
            {
                return db.Users.Where(Predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<UnionManager.Models.DomainModels.User> Select()
        {
            try
            {
                return db.Users.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<UnionManager.Models.DomainModels.User, TResult>> selector)
        {
            try
            {
                return db.Users.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public long GetLastIdentity()
        {
            try
            {
                if (db.Users.Any())
                    return db.Users.OrderByDescending(p => p.Id).First().Id;
                else
                    return 0;
            }
            catch
            {
                return -1;
            }
        }

        public int Save()
        {
            try
            {
                return db.SaveChanges();
            }
            catch
            {
                return -1;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.db != null)
                {
                    this.db.Dispose();
                    this.db = null;
                }
            }
        }

        ~UserRepository()
        {
            Dispose(false);
        }
    }
}