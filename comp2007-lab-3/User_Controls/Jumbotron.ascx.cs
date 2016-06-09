﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace comp2007_lab_3.User_Controls
{
    public partial class Jumbotron : System.Web.UI.UserControl
    {
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public PlaceHolder BodyContent { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            content.Controls.Add(BodyContent);
        }
    }
}