using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UnionManager.Models.Repositories
{
    public class VehicleRepository : IDisposable
    {
        private UnionManager.Models.DomainModels.db_UnionEntities db = null;

        public VehicleRepository()
        {
            db = new DomainModels.db_UnionEntities();
        }

        public bool Add(UnionManager.Models.DomainModels.Vehicle entity, out string message, bool autoSave = true)
        {
            try
            {
                db.Vehicles.Add(entity);
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

        public bool Update(UnionManager.Models.DomainModels.Vehicle entity, out string message, bool autoSave = true)
        {
            try
            {
                UnionManager.Models.DomainModels.Vehicle vehicle = Find(entity.Id);
                db.Entry(vehicle).State = System.Data.Entity.EntityState.Detached;
                db.Vehicles.Attach(entity);
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
            catch(Exception ex)
            {
                message = ex.GetBaseException().Message;
                return false;
            }
        }

        public bool Delete(UnionManager.Models.DomainModels.Vehicle entity, out string message, bool autoSave = true)
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
            catch(Exception ex)
            {
                message = ex.GetBaseException().Message;
                return false;
            }
        }

        public bool Delete(long id, out string message, bool autoSave = true)
        {
            try
            {
                var entity = db.Vehicles.Find(id);
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
            catch(Exception ex)
            {
                message = ex.GetBaseException().Message;
                return false;
            }
        }

        public UnionManager.Models.DomainModels.Vehicle Find(long id)
        {
            try
            {
                return db.Vehicles.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<UnionManager.Models.DomainModels.Vehicle> Where(System.Linq.Expressions.Expression<Func<UnionManager.Models.DomainModels.Vehicle, bool>> Predicate)
        {
            try
            {
                return db.Vehicles.Where(Predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<UnionManager.Models.DomainModels.Vehicle> Select()
        {
            try
            {
                return db.Vehicles.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<UnionManager.Models.DomainModels.Vehicle, TResult>> selector)
        {
            try
            {
                return db.Vehicles.Select(selector);
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
                if (db.Vehicles.Any())
                    return db.Vehicles.OrderByDescending(p => p.Id).First().Id;
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

        ~VehicleRepository()
        {
            Dispose(false);
        }
    }
}