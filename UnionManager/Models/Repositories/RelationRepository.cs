using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnionManager.Models.Repositories
{
    public class RelationRepository : IDisposable
    {
        private UnionManager.Models.DomainModels.db_UnionEntities db = null;

        public RelationRepository()
        {
            db = new DomainModels.db_UnionEntities();
        }

        public bool Add(UnionManager.Models.DomainModels.Relation entity, out string message, bool autoSave = true)
        {
            try
            {
                db.Relations.Add(entity);
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

        public bool Update(UnionManager.Models.DomainModels.Relation entity, out string message, bool autoSave = true)
        {
            try
            {
                UnionManager.Models.DomainModels.Relation relation = Find(entity.Id);
                db.Entry(relation).State = System.Data.Entity.EntityState.Detached;
                db.Relations.Attach(entity);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
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

        public bool Delete(UnionManager.Models.DomainModels.Relation entity, out string message, bool autoSave = true)
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
                var entity = db.Relations.Find(id);
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

        public UnionManager.Models.DomainModels.Relation Find(long id)
        {
            try
            {
                return db.Relations.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<UnionManager.Models.DomainModels.Relation> Where(System.Linq.Expressions.Expression<Func<UnionManager.Models.DomainModels.Relation, bool>> Predicate)
        {
            try
            {
                return db.Relations.Where(Predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<UnionManager.Models.DomainModels.Relation> Select()
        {
            try
            {
                return db.Relations.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<UnionManager.Models.DomainModels.Relation, TResult>> selector)
        {
            try
            {
                return db.Relations.Select(selector);
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
                if (db.Relations.Any())
                    return db.Relations.OrderByDescending(p => p.Id).First().Id;
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

        ~RelationRepository()
        {
            Dispose(false);
        }
    }
}