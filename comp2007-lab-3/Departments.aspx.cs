using comp2007_lab_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Dynamic;
namespace comp2007_lab_3
{
    public partial class Departments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["SortColumn"] = "DepartmentID";
                Session["SortDirection"] = "ASC";
                this.get_department();

            }
        }
        protected void get_department()
        {
            string sortString = Session["SortColumn"].ToString() + " " + Session["SortDirection"];
            using (DefaultConnection db = new DefaultConnection())
            {
                var department = (from alldepartments in db.Departments
                                  select alldepartments);
                DepartmentsGridView.DataSource = department.AsQueryable().OrderBy(sortString).ToList();
                DepartmentsGridView.DataBind();

            }
        }
        protected void DepartmentsGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            Session["SortColumn"] = e.SortExpression;
            this.get_department();
            Session["SortDirection"] = Session["SortDirection"].ToString() == "Asc" ? "DESC" : "ASC";
        }
        protected void DepartmentsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (IsPostBack)
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    LinkButton linkbutton = new LinkButton();
                    for (int index = 0; index < DepartmentsGridView.Columns.Count; index++)
                    {
                        if (DepartmentsGridView.Columns[index].SortExpression == Session["SortColumn"].ToString())
                        {
                            if (Session["SortDirection"].ToString() == "Asc")
                            {
                                linkbutton.Text = "<i class='fa fa-caret-up fa-lg'></i>";
                            }
                            else
                            {
                                linkbutton.Text = "<i class='fa fa-caret-down fa-lg'></i>";
                            }
                            e.Row.Cells[index].Controls.Add(linkbutton);
                        }
                    }
                }
            }
        }

    

        protected void DepartmentsGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int selectedrow = e.RowIndex;
            int departmentId = Convert.ToInt32(DepartmentsGridView.DataKeys[selectedrow].Values["DepartmentID"]);

            using (DefaultConnection db = new DefaultConnection())
            {
                Department department = (from departmentrecord in db.Departments
                                         where departmentrecord.DepartmentID == departmentId
                                         select departmentrecord).FirstOrDefault();
                db.Departments.Remove(department);
                db.SaveChanges();
                this.get_department();
            }
        }
    }
}