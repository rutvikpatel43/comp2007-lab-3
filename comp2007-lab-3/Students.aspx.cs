﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Dynamic;
// using statements that are required to connect to EF DB
using comp2007_lab_3.Models;
using System.Web.ModelBinding;

namespace comp2007_lab_3
{
    public partial class Students : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // if loading the page for the first time, populate the student grid
            if (!IsPostBack)
            {
                Session["SortColumn"] = "StudentID";
                Session["SortDirection"] = "ASC";
                // Get the student data
                this.GetStudents();
            }
        }

        /**
         * <summary>
         * This method gets the student data from the DB
         * </summary>
         * 
         * @method GetStudents
         * @returns {void}
         */
        protected void GetStudents()
        {
            string sortString = Session["SortColumn"].ToString() + " " + Session["SortDirection"].ToString();
            // connect to EF
            using (DefaultConnection db = new DefaultConnection())
            {
                // query the Students Table using EF and LINQ
                var Students = (from allStudents in db.Students
                                select allStudents);

                // bind the result to the GridView
                StudentsGridView.DataSource = Students.AsQueryable().OrderBy(sortString).ToList();
                StudentsGridView.DataBind();
            }
        }
        /**
          * <summary>
          * This method gets the student data from the DB
          * </summary>
          * 
          * @method GetStudents
          * @returns {void}
          */
        protected void StudentsGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // store which row was clicked
            int selectedRow = e.RowIndex;

            // get the selected StudentID using the Grid's DataKey Collection
            int StudentID = Convert.ToInt32(StudentsGridView.DataKeys[selectedRow].Values["StudentID"]);

            // use EF to find the selected student from DB and remove it
            using (DefaultConnection db = new DefaultConnection())
            {
                Student deletedStudent = (from studentRecords in db.Students
                                          where studentRecords.StudentID == StudentID
                                          select studentRecords).FirstOrDefault();

                // perform the removal in the DB
                db.Students.Remove(deletedStudent);
                db.SaveChanges();

                // refresh the grid
                this.GetStudents();
            }
            }

        protected void StudentsGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            StudentsGridView.PageIndex = e.NewPageIndex;

            // refresh the grid
            this.GetStudents();
        }

        protected void PageSizeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // set new page size
            StudentsGridView.PageSize = Convert.ToInt32(PageSizeDropDownList.SelectedValue);

            this.GetStudents();
        }

        protected void StudentsGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            Session["SortColumn"] = e.SortExpression;
            // refresh grid
            this.GetStudents();
            //toggle the direction
            Session["SortDirection"] = Session["SortDirection"].ToString() == "Asc" ? "DESC" : "ASC";
        }

        protected void StudentsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(IsPostBack)
            {
                if(e.Row.RowType==DataControlRowType.Header)
                {
                    LinkButton linkButton = new LinkButton();
                    for(int index=0;index<StudentsGridView.Columns.Count;index++)
                    {
                        if(StudentsGridView.Columns[index].SortExpression==Session["SortColumn"].ToString())
                        {
                            if(Session["SortDirection"].ToString() == "Asc")
                            {
                                linkButton.Text = "<i class='fa fa-caret-up fa-lg'></i>";
                            }
                            else
                            {
                                linkButton.Text = "<i class='fa fa-caret-down fa-lg'></i>";
                            }
                            e.Row.Cells[index].Controls.Add(linkButton);
                        }
                    }

                }
            }
        }
    }
}