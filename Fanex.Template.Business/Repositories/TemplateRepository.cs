// -----------------------------------------------------------------------
// <copyright file="ResetWinningRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Template.Business.Repositories
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.AccountingReports.SqLiteDatabase;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Template.Business.Business;
    using Fanex.Template.Business.Enums;
    using Fanex.Template.Business.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TemplateRepository : ITemplateRepository
    {
        private SqLite _db = null;

        public TemplateRepository()
        {
            _db = new SqLite(AppSettings.ConnectionString);
        }

        public SMAReportModel GetCurrentTask()
        {
            SMAReportModel model = null;
            try
            {
                string sql = "select * from log where Status = "+ (int)Status.Running +" LIMIT 1";
                DataTable dataTable = _db.GetDataTable(sql);
                
                if (dataTable.Rows.Count == 0)
                    return model;               

                DataRow row = dataTable.Rows[0];
                model = new SMAReportModel() 
                {
                    Id = Converter.ToInt(row["Id"]),
                    TaskStatus = (Status)Converter.ToInt(row["Status"]),
                    CreatedDate = Converter.ToString2(row["CreatedDate"]),
                    UpdatedDate = Converter.ToString2(row["UpdatedDate"]),
                    UserId = Converter.ToInt(row["UserId"]),
                    UserName = Converter.ToString2(row["UserName"]),
                    ResetOnDate = Converter.ToString2(row["ResetOnDate"])
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return model;
        }

        public void UpdateTaskStatus(int Id, Status status)
        {
            try
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("Status", ((int)status).ToString()); 
                data.Add("UpdatedDate", DateTime.Now.ToString());

                string where = " Id = " + Id;
                _db.Update("Log", data, where);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void MoveTaskToArchive(int Id)
        {
            try
            {
                SMAReportModel model = GetTaskById(Id);
                if (model == null)
                    return;

                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("Id", model.Id.ToString());
                data.Add("Status", Converter.ToInt(model.TaskStatus).ToString());
                data.Add("CreatedDate", model.CreatedDate);
                data.Add("UpdatedDate", model.UpdatedDate);
                data.Add("UserId", model.UserId.ToString());
                data.Add("UserName", model.UserName);
                data.Add("ResetOnDate", model.ResetOnDate);

                _db.Insert("Log_Archive", data);

                string where = " Id = " + Id;
                _db.Delete("Log", where);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public SMAReportModel GetTaskById(int Id)
        {
            SMAReportModel model = null;
            try
            {
                string sql = "select * from log where Id = " + Id;
                DataTable dataTable = _db.GetDataTable(sql);

                if (dataTable.Rows.Count == 0)
                {
                    return model;
                }
                    
                DataRow row = dataTable.Rows[0];
                model = new SMAReportModel()
                {
                    Id = Converter.ToInt(row["Id"]),
                    TaskStatus = (Status)Converter.ToInt(row["Status"]),
                    CreatedDate = Converter.ToString2(row["CreatedDate"]),
                    UpdatedDate = Converter.ToString2(row["UpdatedDate"]),
                    UserId = Converter.ToInt(row["UserId"]),
                    UserName = Converter.ToString2(row["UserName"]),
                    ResetOnDate = Converter.ToString2(row["ResetOnDate"])
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return model;
        }


        public Result ResetWinning(int Id)
        {
            Result result = new Result();
            try
            {
                SMAReportModel model = GetTaskById(Id);

                if (model == null)
                {
                    result.ErrCode = 100;
                    result.ErrMsg = "There is no data to reset";

                    return result;
                }

                Hashtable parameters = new Hashtable();
                parameters.Add("fromdate", model.ResetOnDate);
                parameters.Add("todate", model.ResetOnDate);
                parameters.Add("mode", 1);////Reset
                parameters.Add("errcode", 0);
                parameters.Add("errmsg", string.Empty);

                using (ObjectDb db = new ObjectDb("Task_ResetWinning_Non3rdParty"))
                {
                    db.AssignParamValues(parameters);
                    db.ExecuteNonQuery();

                    Hashtable output = db.GetOuputParams();
                    result.ErrCode = Converter.ToInt(output["errcode"]);
                    result.ErrMsg = Converter.ToString2(output["errmsg"]);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }


        public int InsertTask(SMAReportModel task)
        {
            int Id = 0;
            try
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("Status", ((int)Status.Running).ToString());
                data.Add("CreatedDate", DateTime.Now.ToString());
                data.Add("UserId", task.UserId.ToString());
                data.Add("UserName", task.UserName);
                data.Add("ResetOnDate", task.ResetOnDate);

                Id = Converter.ToInt(_db.Insert("Log", data));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Id;
        }


        public Status GetSatatusByTaskId(int Id)
        {
            Status status = Status.Running;
            try
            {
                SMAReportModel model = GetTaskById(Id);
                if (model == null)
                {
                    string sql = "select * from log_archive where Id = " + Id;
                    DataTable dataTable = _db.GetDataTable(sql);

                    if (dataTable.Rows.Count == 0)
                    {
                        status = Status.CannotFound;
                        return status;
                    }

                    DataRow row = dataTable.Rows[0];
                    model = new SMAReportModel()
                    {
                        Id = Converter.ToInt(row["Id"]),
                        TaskStatus = (Status)Converter.ToInt(row["Status"])                        
                    };
                }

                status = model.TaskStatus;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;            
        }
    }
}
