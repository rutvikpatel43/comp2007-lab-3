using comp2007_lab_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace comp2007_lab_3
{
    public partial class DepartmentDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack&&Request.QueryString.Count>0)
            {
                this.get_department();
            }
        }
        protected void get_department()
        {
            int departmentID = Convert.ToInt32(Request.QueryString["DepartmentID"]);
            using (DefaultConnection db = new DefaultConnection())
            {
                Department updatedDepartment = (from updatedepartment in db.Departments
                                                where updatedepartment.DepartmentID == departmentID
                                                select updatedepartment).FirstOrDefault();
                if(updatedDepartment!=null)
                {
                    NameTextBox.Text = updatedDepartment.Name;
                    BudgetTextBox.Text = Convert.ToString(updatedDepartment.Budget);
                }
                
            }
        }
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Departments.aspx");
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            Department department = new Department();
            using (DefaultConnection db = new DefaultConnection())
            {
                int departmentId = 0;
                if(Request.QueryString.Count>0)
                {
                    departmentId = Convert.ToInt32(Request.QueryString["DepartmentID"]);
                    department = (from depart in db.Departments
                                  where depart.DepartmentID == departmentId
                                  select depart).FirstOrDefault();
                }
                department.Name = NameTextBox.Text;
                department.Budget = Convert.ToDecimal(BudgetTextBox.Text.ToString());
                if (departmentId == 0)
                    db.Departments.Add(department);
                db.SaveChanges();
                Response.Redirect("~/Departments.aspx");
            }
        }
    }
}