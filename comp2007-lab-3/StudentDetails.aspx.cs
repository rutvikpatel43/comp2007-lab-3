using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Dynamic;
// using statements required for EF DB access
using comp2007_lab_3.Models;
using System.Web.ModelBinding;

namespace comp2007_lab_3
{
    public partial class StudentDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack&&Request.QueryString.Count>0)
            {
                this.GetStudents();
            }
        }

        protected void GetStudents()
        {
            int StudentID = Convert.ToInt32(Request.QueryString["StudentID"]);
            //connect to database
            using (DefaultConnection db = new DefaultConnection())
            {
                Student updatedStudent = (from student in db.Students
                                          where student.StudentID == StudentID
                                          select student).FirstOrDefault();
                if(updatedStudent!=null)
                {
                    LastNameTextBox.Text = updatedStudent.LastName;
                    FirstNameTextBox.Text = updatedStudent.FirstMidName;
                    EnrollmentDateTextBox.Text = updatedStudent.EnrollmentDate.ToString("yyyy-MM-dd");
                }
            }
        }
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            // Redirect back to Students page
            Response.Redirect("~/Students.aspx");
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {

            // Use EF to connect to the server
            using (DefaultConnection db = new DefaultConnection())
            {
                // use the Student model to create a new student object and
                // save a new record
                Student newStudent = new Student();

                int studentID = 0;
                if(Request.QueryString.Count>0)
                {
                    studentID = Convert.ToInt32(Request.QueryString["studentID"]);

                    newStudent=(from student in db.Students
                                where student.StudentID == studentID
                                select student).FirstOrDefault();
                }
                // add form data to the new student record
                newStudent.LastName = LastNameTextBox.Text;
                newStudent.FirstMidName = FirstNameTextBox.Text;
                newStudent.EnrollmentDate = Convert.ToDateTime(EnrollmentDateTextBox.Text);

                // use LINQ to ADO.NET to add / insert new student into the database
                //check to see a new student is added
                if (studentID == 0)
                {
                    db.Students.Add(newStudent);
                }


                // save our changes
                db.SaveChanges();

                // Redirect back to the updated students page
                Response.Redirect("~/Students.aspx");
            }
        }
    }
}