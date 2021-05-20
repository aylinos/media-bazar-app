using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaBazarProject.Models;

namespace MediaBazarProject
{
    public partial class FormManager : Form
    {
        StatisticsController stController;
        UserController uc;
        StockController sc;
        DepartmentController dc; 
        User loggedInUser;
        int currentID;
        int loggedUserId;
        ScheduleController scController;
        int stockID;
        public FormManager(int id)
        {
            InitializeComponent();
            stController = new StatisticsController();
            scController = new ScheduleController();
            scController.GetShifts();
            lblTodayDay.Text = DateTime.Today.ToString("d/M/yyyy");
            uc = new UserController();
            dc = new DepartmentController();
            sc = new StockController();
            currentID = 0;
            loggedUserId = id;
        }


        private void FormManager_Load(object sender, EventArgs e)
        {
            scController.CheckForUpdateSchedule();
            uc.ExtractAllUsers();
            loggedInUser = uc.GetUser(loggedUserId);
            currentID = 0;
            stockID = 0;
            lblUserName.Text = loggedInUser.Username;

            foreach (var item in stController.GetAllDepartments())
            {
                cmxCategoryStock.AddItem(item);
            }
            UpdateListView();

        }

        bool menuExpanded = false;
        private void MouseDetect_Tick(object sender, EventArgs e)
        {
            if (!bunifuTransition1.IsCompleted) { return; }
            if (panelNavBar.ClientRectangle.Contains(PointToClient(Control.MousePosition)))
            {
                if (!menuExpanded)
                {
                    menuExpanded = true;
                    panelNavBar.Visible = false;
                    panelNavBar.Width = 290;
                    bunifuTransition1.Show(panelNavBar);
                }
            }
            else
            {
                if (menuExpanded)
                {
                    menuExpanded = false;
                    panelNavBar.Visible = false;
                    panelNavBar.Width = 80;
                    bunifuTransition1.Show(panelNavBar);
                }
            }

        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {
            if (btnEmplStat.Visible == true)
            {
                btnEmplStat.Visible = false;
                btnProductStat.Visible = false;
                btnGeneralStat.Visible = false;
            }
            else
            {
                btnEmplStat.Visible = true;
                btnProductStat.Visible = true;
                btnGeneralStat.Visible = true;
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            tabControlManager.SelectTab(tpHomeManager);
        }

        private void btnEmployeeMenu_Click(object sender, EventArgs e)
        {
            tabControlManager.SelectTab(tpEmpl);

            //Load all employees in the listview
            uc.ExtractAllUsers();
            UpdateListView();
            if (lvEmployee.Items.Count == 0)
            {
                CustomMessageBoxController.ShowMessage("No employees found!", MessageBoxButtons.OK);
            }
            if (searchEmpl.text != "Search by name")
            {
                searchEmpl.text = "Search by name";
            }

            //Load departments in combobox
            LoadDepartmentsInComboBox();
            //cmxCategoryEmpl.Clear();
            //foreach (var item in stController.GetAllDepartments())
            //{
            //    cmxCategoryEmpl.AddItem(item);
            //}

            editPanelUser.Visible = false;
            lvEmployee.Enabled = true;
            lvEmployee.HideSelection = true;
        }

        private void LoadDepartmentsInComboBox()
        {
            cmxCategoryEmpl.Clear();
            foreach (var item in stController.GetAllDepartments())
            {
                cmxCategoryEmpl.AddItem(item);
            }
        }
        private void btnStocksMenu_Click(object sender, EventArgs e)
        {
            tabControlManager.SelectTab(tpStocks);
            lvStock.HideSelection = true;
        }

        private void btnStatEmployee_Click(object sender, EventArgs e)
        {
            tabControlManager.SelectTab(tpStat);
        }


        private void btnLogOut_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEditStock_Click(object sender, EventArgs e)
        {
            bunifuDropdownEditProductDepartment.Clear();
            if (stockID == 0)
                CustomMessageBoxController.ShowMessage("Choose a stock to edit first!", MessageBoxButtons.OK);
            else
            {
                tbEditProductName.Text = sc.GetStock(stockID).Name;
                foreach (var item in stController.GetAllDepartments())
                {
                    bunifuDropdownEditProductDepartment.AddItem(item);
                }
                //bunifuDropdownEditProductDepartment.selectedIndex = Convert.ToInt32(sc.GetStock(stockID).Department.Id) - 1;
                bunifuDropdownEditProductDepartment.selectedIndex = sc.GetStock(stockID).Department.Id - 1;
                panelEditProduct.Visible = true;
            }
        }

        private void btnSavePr_Click(object sender, EventArgs e)
        {

            if (sc.EditStock(stockID, bunifuDropdownEditProductDepartment.selectedValue))
            {
                CustomMessageBoxController.ShowMessage($"Department of {sc.GetStock(stockID).Name} successfully updated!", MessageBoxButtons.OK);
                uc.ExtractAllUsers();
                UpdateListView();
            }
            else
            {
                CustomMessageBoxController.ShowMessage("Failed to update department!", MessageBoxButtons.OK);
            }
            editPanelUser.Visible = false;
            panelEditProduct.Visible = false;

            //if (uc.EditUser(currentID, bunifuDropdownEditDepartment.selectedValue))
            //{
            //    CustomMessageBoxController.ShowMessage($"Department of employee {currentID} successfully updated!", MessageBoxButtons.OK);
            //    uc.ExtractAllUsers();
            //    UpdateListView();
            //}
            //else
            //{
            //    CustomMessageBoxController.ShowMessage("Failed to update department!", MessageBoxButtons.OK);
            //}
            //editPanelUser.Visible = false;

        }

        private void btnCancelPr_Click(object sender, EventArgs e)
        {
            panelEditProduct.Visible = false;
        }

        private void subMenuStatistics1_Load(object sender, EventArgs e)
        {

        }

        private void btnGen1_Click(object sender, EventArgs e)
        {
            tabControlStatDesign.SelectTab(tpGeneral);
            lblDaysNumber.Text = stController.GetTotalDaysSinceOpening().ToString();
            lbTotprNumber.Text = stController.GetTotalProducts().ToString();
            lbltotEmplNumber.Text = stController.GetTotalUsers().ToString();
        }

        private void btnGen2_Click(object sender, EventArgs e)
        {
            tabControlStatDesign.SelectTab(tpIncome);
            lblTotalIncome.Text = stController.GetTotalIncome().Values.First().ToString() + "$";
            lblAvgIncome.Text = stController.GetAverageIncomePerDay().ToString() + "$";
        }

        private void btnGen3_Click(object sender, EventArgs e)
        {
            tabControlStatDesign.SelectTab(tpExpenses);
            int admin = stController.GetSalarySumPerRole()["ADMINISTRATOR"];
            int manager = stController.GetSalarySumPerRole()["MANAGER"];
            int employee = stController.GetSalarySumPerRole()["EMPLOYEE"];
            lblAdminSal.Text = admin.ToString() + "$";
            lblManagerSal.Text = manager.ToString() + "$";
            lblEmplSal.Text = employee.ToString() + "$";
            lblTotalMoneyOnSalary.Text = (admin + manager + employee).ToString() + "$";
            GaugeAdmin.Value = Convert.ToInt32(((double)admin / (double)(admin + manager + employee)) * 100);
            GaugeManager.Value = Convert.ToInt32(((double)manager / (double)(admin + manager + employee)) * 100);
            GaugeEmpl.Value = Convert.ToInt32(((double)employee / (double)(admin + manager + employee)) * 100);
        }

        private void btnPr1_Click(object sender, EventArgs e)
        {
            tabControlStatDesign.SelectTab(tpMostSoldPr);
            Dictionary<string, int> mostSold= stController.GetMostSoldProduct();
            lblMostSoldPr.Text = mostSold.Keys.First();
            lblMostSoldQuantity.Text = mostSold.Values.First().ToString();
            FillDropDepartmentStatisctics();
        }
        private void FillDropDepartmentStatisctics()
        {
            dropDepartment.Clear();
            foreach (var item in stController.GetAllDepartments())
            {
                dropDepartment.AddItem(item);
            }
        }
        
        private void dropDepartment_onItemSelected(object sender, EventArgs e)
        {          
            try
            {
                if (chartMostSold.Series["mostSold"].Points.Count() < 3)
                {
                    chartMostSold.Series["mostSold"].Points.AddXY(dropDepartment.selectedValue.Trim(),
                        stController.GetMostSoldProduct(dropDepartment.selectedValue));
                    dropDepartment.RemoveAt(dropDepartment.selectedIndex);
                }
                else
                {
                    CustomMessageBoxController.ShowMessage("You can compare up to three items.", MessageBoxButtons.OK);
                    chartMostSold.Series["mostSold"].Points.Clear();
                    FillDropDepartmentStatisctics();
                }
            }
            catch
            {
                CustomMessageBoxController.ShowMessage("No statistics are available for this department.", MessageBoxButtons.OK);
            }
        }

        private void btnEmpl4_Click(object sender, EventArgs e)
        {
            tabControlStatDesign.SelectTab(tpReasonsForQuiting);
            Dictionary<string, int> reasonForQuiting = stController.GetReasonsForQuiting();
            int rowsCount = reasonForQuiting.Count();
            int allreasons = stController.GetTotalNumberOfQuitedEmpl();

            if (rowsCount > 0)
            {
                lblReason1.Text = reasonForQuiting.ElementAt(0).Key;
                lblReasonppl1.Text = reasonForQuiting.ElementAt(0).Value.ToString() + "ppl";
                progressReason1.Value = Convert.ToInt32(((double)reasonForQuiting.ElementAt(0).Value / (double)allreasons) * 100);
                if (rowsCount > 1)
                {
                    lblReason2.Text = reasonForQuiting.ElementAt(1).Key;
                    lblReasonPpl2.Text = reasonForQuiting.ElementAt(1).Value.ToString() + "ppl";
                    progressReason2.Value = Convert.ToInt32(((double)reasonForQuiting.ElementAt(1).Value / (double)allreasons) * 100);

                    if (rowsCount > 2)
                    {
                        lblReason3.Text = reasonForQuiting.ElementAt(2).Key;
                        lblReasonPpl3.Text = reasonForQuiting.ElementAt(2).Value.ToString() + "ppl";
                        progressReason3.Value = Convert.ToInt32(((double)reasonForQuiting.ElementAt(2).Value / (double)allreasons) * 100);
                        if (rowsCount > 3)
                        {
                            lblReason4.Text = reasonForQuiting.ElementAt(3).Key;
                            lblReasonPpl4.Text = reasonForQuiting.ElementAt(3).Value.ToString() + "ppl";
                            progressReason4.Value = Convert.ToInt32(((double)reasonForQuiting.ElementAt(3).Value / (double)allreasons) * 100);
                        }
                        else { panelReason4.Visible = false; }
                    }
                    else
                    {
                        panelReason3.Visible = false;
                        panelReason4.Visible = false;
                    }
                }
                else
                {
                    panelreason2.Visible = false;
                    panelReason3.Visible = false;
                    panelReason4.Visible = false;
                }
            }
            else
            {
                lblReason1.Text = "There are no records in the databse!";
                lblReasonppl1.Text = "";
                progressReason1.Visible = false;
            }
        }

        private void btnEmpl1_Click(object sender, EventArgs e)
        {
            tabControlStatDesign.SelectTab(tpUsersPerRole);
            Dictionary<string, int> usersPerRole = stController.GetUsersPerRole();
            try
            {
                pieUsersPerRole.Series["s1"].IsValueShownAsLabel = true;
                pieUsersPerRole.Series["s1"].Points.AddXY(usersPerRole.ElementAt(0).Key, usersPerRole.ElementAt(0).Value);
                pieUsersPerRole.Series["s1"].Points.AddXY(usersPerRole.ElementAt(1).Key, usersPerRole.ElementAt(1).Value);
                pieUsersPerRole.Series["s1"].Points.AddXY(usersPerRole.ElementAt(2).Key, usersPerRole.ElementAt(2).Value);
            }
            catch (Exception) { }

            Dictionary<string, int> salaryPerRole = stController.GetAverageSalaryPerRoles();
            try
            {
                chartSalaryPerRole.Series["s2"].IsValueShownAsLabel = true;
                chartSalaryPerRole.Series["s2"].Points.AddXY(salaryPerRole.ElementAt(0).Key, salaryPerRole.ElementAt(0).Value);
                chartSalaryPerRole.Series["s2"].Points.AddXY(salaryPerRole.ElementAt(1).Key, salaryPerRole.ElementAt(1).Value);
                chartSalaryPerRole.Series["s2"].Points.AddXY(salaryPerRole.ElementAt(2).Key, salaryPerRole.ElementAt(2).Value);
            }
            catch (Exception) { }

        }

        private void btnEmpl2_Click(object sender, EventArgs e)
        {
            tabControlStatDesign.SelectTab(tpGenderDepartmentSepartation);
            //TODO on multiple click the values keep addidng
            Dictionary<string, int> gender = stController.GetUsersPerGender();
            try
            {
                pieGender.Series["gender"].IsValueShownAsLabel = true;
                pieGender.Series["gender"].Points.AddXY(gender.ElementAt(0).Key, gender.ElementAt(0).Value);
                pieGender.Series["gender"].Points.AddXY(gender.ElementAt(1).Key, gender.ElementAt(1).Value);
                pieGender.Series["gender"].Points.AddXY(gender.ElementAt(2).Key, gender.ElementAt(2).Value);
            }
            catch (Exception) { }
            FillItemsEmployeeStatisctics();

        }

        private void FillItemsEmployeeStatisctics()
        {
            cmbDepartments.Clear();
            foreach (var item in stController.GetAllDepartments())
            {
                cmbDepartments.AddItem(item);
            }
        }
        private void cmbDepartments_onItemSelected(object sender, EventArgs e)
        {
            //TODO add up to three values to compare
            

            try
            {
                if (chartEmplPerDept.Series["empPerDept"].Points.Count() < 3)
                {
                    chartEmplPerDept.Series["empPerDept"].Points.AddXY(cmbDepartments.selectedValue,
                stController.GetEmployeesInDepartment(cmbDepartments.selectedValue));
                    cmbDepartments.RemoveAt(cmbDepartments.selectedIndex);
                }
                else
                {
                    CustomMessageBoxController.ShowMessage("You can compare up to three items.", MessageBoxButtons.OK);
                    chartEmplPerDept.Series["empPerDept"].Points.Clear();
                    FillItemsEmployeeStatisctics();
                }
            }
            catch
            {
                CustomMessageBoxController.ShowMessage("No statistics are available for this department.", MessageBoxButtons.OK);
            }
        }

        private void btnEmpl5_Click(object sender, EventArgs e)
        {
            tabControlStatDesign.SelectTab(tpWorkTimePastEmpl);
            Dictionary<string, int> worktime = stController.CalculateWorktime();
            try
            {
                pieWorktime.Series["worktime"].IsValueShownAsLabel = true;
                pieWorktime.Series["worktime"].Points.AddXY(worktime.ElementAt(0).Key, worktime.ElementAt(0).Value);
                pieWorktime.Series["worktime"].Points.AddXY(worktime.ElementAt(1).Key, worktime.ElementAt(1).Value);
                pieWorktime.Series["worktime"].Points.AddXY(worktime.ElementAt(2).Key, worktime.ElementAt(2).Value);
                pieWorktime.Series["worktime"].Points.AddXY(worktime.ElementAt(3).Key, worktime.ElementAt(3).Value);
                pieWorktime.Series["worktime"].Points.AddXY(worktime.ElementAt(4).Key, worktime.ElementAt(4).Value);
            }
            catch (Exception) { }
        }


        private void btnPr2_Click(object sender, EventArgs e)
        {
            tabControlStatDesign.SelectTab(tpIncomeStat);
            Dictionary<string, int> income = stController.GetIncomeStats();
            try
            {
                btnTotal.Text = $"Total income: ${income.Values.First()}";
                btnCost.Text = $"All costs of products: ${income.Keys.First()}";
                double profit = income.Values.First() - Convert.ToDouble(income.Keys.First());
                btnRevenue.Text = $"Total profit of the store is ${profit}";
            }
            catch (Exception) { }
        }

        private void btnPr3_Click(object sender, EventArgs e)
        {
            tabControlStatDesign.SelectTab(tpPrAvailability);
            cmxDepartmentProducts.Clear();
            stController.GetAllDepartments().ForEach(dpt => cmxDepartmentProducts.AddItem(dpt));
        }

        private void btnPr4_Click(object sender, EventArgs e)
        {
            tabControlStatDesign.SelectTab(tpProfitPerPr);
            cmxProfitDept.Clear();
            stController.GetAllDepartments().ForEach(dpt => cmxProfitDept.AddItem(dpt));
        }

        private void btnPr5_Click(object sender, EventArgs e)
        {
            tabControlStatDesign.SelectTab(tpStockRequests);
            Dictionary<string, int> requests = stController.GetStockRequestStats();
            try
            {
                foreach (var dept in stController.GetStockRequestStats())
                {
                    chartPrRequests.Series["request"].Points.AddXY(dept.Key, dept.Value);
                }

            }
            catch (Exception) { }
        }

        private void cmxDepartmentProducts_onItemSelected(object sender, EventArgs e)
        {
            cmxProductDepartment.Clear();
            stController.GetProductsByDepartment(cmxDepartmentProducts.selectedValue).ForEach(pr => cmxProductDepartment.AddItem(pr));
            //foreach (var item in stController.GetProductsByDepartment(cmxDepartmentProducts.selectedValue))
            //{
            //    cmxProductDepartment.AddItem(item);
            //}
        }

        private void cmxProductDepartment_onItemSelected(object sender, EventArgs e)
        {
            Tuple<int, int> quantity = stController.GetQuantityStats(cmxProductDepartment.selectedValue);
            int value = (int)(((double)quantity.Item1 / (double)quantity.Item2) * 100);
            if (value < 100)
            {
                gaugeProductAvailability.Value = value;
            }
            else
            {
                gaugeProductAvailability.Value = 100;
            }

        }

        private void cmxProfitDept_onItemSelected(object sender, EventArgs e)
        {
            cmxProfitProduct.Clear();
            stController.GetProductsByDepartment(cmxProfitDept.selectedValue).ForEach(pr => cmxProfitProduct.AddItem(pr));
        }

        private void cmxProfitProduct_onItemSelected(object sender, EventArgs e)
        {
            decimal profitPerProduct = stController.GetProductProfit(cmxProfitProduct.selectedValue);
            int totalProfit = stController.GetTotalIncome().Values.First();
            circleProfit.Value = (int)(profitPerProduct / (decimal)(totalProfit) * 100);
            lblProfitPerPr.Text = "$ " + profitPerProduct.ToString();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (currentID == 0)
            {
                CustomMessageBoxController.ShowMessage("Choose an employee to edit first!", MessageBoxButtons.OK);
            }
            else
            {
                //Load departments in combobox
                bunifuDropdownEditDepartment.Clear();
                foreach (string depName in stController.GetAllDepartments())
                {
                    bunifuDropdownEditDepartment.AddItem(depName);
                }
                editPanelUser.Visible = true;
                tbEditName.Text = uc.GetUser(currentID).FirstName + " " + uc.GetUser(currentID).LastName;
                //bunifuDropdownEditDepartment.selectedIndex = Convert.ToInt32(uc.GetUser(currentID).Department);
                bunifuDropdownEditDepartment.selectedIndex = stController.GetAllDepartments().IndexOf(uc.GetUser(currentID).Dep.DptName);
                tbEditName.Enabled = false;
            }
        }

        private void btnCancelUser_Click(object sender, EventArgs e)
        {
            editPanelUser.Visible = false;
        }

        private void brnSaveUser_Click(object sender, EventArgs e)
        {
            if (uc.EditUser(currentID, bunifuDropdownEditDepartment.selectedValue))
            {
                CustomMessageBoxController.ShowMessage($"Department of employee {currentID} successfully updated!", MessageBoxButtons.OK);
                uc.ExtractAllUsers();
                UpdateListView();
            }
            else
            {
                CustomMessageBoxController.ShowMessage("Failed to update department!", MessageBoxButtons.OK);
            }
            editPanelUser.Visible = false;
        }

        private void btnEmplStat_Click(object sender, EventArgs e)
        {
            tabControlManager.SelectTab(tpStat);
            tabControlInnerMenu.SelectTab(tpEmployee);
            tabControlStatDesign.SelectTab(tpNoneSelected);
        }

        private void btnProductStat_Click(object sender, EventArgs e)
        {
            tabControlManager.SelectTab(tpStat);
            tabControlInnerMenu.SelectTab(tpProducts);
            tabControlStatDesign.SelectTab(tpNoneSelected);
        }

        private void btnGeneralStat_Click(object sender, EventArgs e)
        {
            tabControlManager.SelectTab(tpStat);
            tabControlInnerMenu.SelectTab(tpCompany);
            tabControlStatDesign.SelectTab(tpNoneSelected);
        }

        private void btnAddSchedule_Click(object sender, EventArgs e)
        {
            pnlAddShift.Visible = true;
            foreach (var emp in uc.Employees)
            {
                cmxAvailableEmpl.AddItem(emp.Id + ":" + emp.Username + " " + emp.Dep.DptName);
            }
            //Assign shifts and save them
            //CustomMessageBoxController.ShowMessage("Generating week schedule. This will take a few seconds.", MessageBoxButtons.OK);
            //scController.AssignShifts();
            //scController.SaveWeeklyShifts();
            //scController.Log();
            //CustomMessageBoxController.ShowMessage("Assigned shifts.", MessageBoxButtons.OK);
        }

        private void btnSaveShift_Click(object sender, EventArgs e)
        {
            pnlAddShift.Visible = false;
            string[] text = cmxAvailableEmpl.selectedValue.Split(':');
            int id = int.Parse(text[0]);
            Shift newShift = new Shift(0, (Time)cmxShift.selectedIndex,datepicker.selectedIndex);
            newShift.Users.Add(uc.GetUser(id));
            scController.SaveShift(newShift, uc.GetUser(id));
            CustomMessageBoxController.ShowMessage("Shift Added!", MessageBoxButtons.OK);
        }

        private void btnCancelShift_Click(object sender, EventArgs e)
        {
            pnlAddShift.Visible = false;
        }

        private void UpdateListView()
        {
            lvEmployee.Items.Clear();
            lvStock.Items.Clear();
            sc.GetStocks();

            foreach (var stock in sc.Stocks)
            {
                string[] row = { stock.Id.ToString(), stock.Name, stock.Department.DptName.ToString(), stock.SupplyCost.ToString(), stock.Price.ToString(), stock.Quantity.ToString() };
                lvStock.Items.Add(new ListViewItem(row));
            }


            foreach (User u in uc.Employees)
            {
                string names = u.FirstName + " " + u.LastName;
                string dep = u.Dep.DptName;
                string[] row = { Convert.ToString(u.Id), names, dep, Convert.ToString(u.Salary), u.Email };
                ListViewItem item = new ListViewItem(row);
                lvEmployee.Items.Add(item);
            }
        }

        private void searchEmpl_OnTextChange(object sender, EventArgs e)
        {
            if (searchEmpl.text != "Search by name")
            {
                lvEmployee.Items.Clear();
                foreach (User u in uc.GetUsers(searchEmpl.text))
                {
                    if (u.Role == Role.EMPLOYEE)
                    {
                        string names = u.FirstName + " " + u.LastName;
                        //string department = Convert.ToString(u.Department);
                        string department = u.Dep.DptName;
                        string[] row = { Convert.ToString(u.Id), names, department, Convert.ToString(u.Salary), u.Email };
                        ListViewItem item = new ListViewItem(row);
                        lvEmployee.Items.Add(item);
                    }
                }
            }
        }

        private void cmxCategoryEmpl_onItemSelected(object sender, EventArgs e)
        {
            lvEmployee.Items.Clear();
            foreach (User u in uc.GetUsersByDep(cmxCategoryEmpl.selectedValue))
            {
                string names = u.FirstName + " " + u.LastName;
                //string department = Convert.ToString(u.Department);
                string department = u.Dep.DptName;
                string[] row = { Convert.ToString(u.Id), names, department, Convert.ToString(u.Salary), u.Email };
                ListViewItem item = new ListViewItem(row);
                lvEmployee.Items.Add(item);
            }
        }

        private void searchEmpl_Enter(object sender, EventArgs e)
        {
            searchEmpl.text = "";
        }

        private void searchEmpl_Leave(object sender, EventArgs e)
        {
            searchEmpl.text = "Search by name";
        }


        private void lvEmployee_MouseClick(object sender, MouseEventArgs e)
        {
            ListView.SelectedIndexCollection indices = lvEmployee.SelectedIndices;

            if (indices.Count > 0)
            {
                var item = lvEmployee.SelectedItems[0];
                currentID = Convert.ToInt32(item.SubItems[0].Text);
            }
            indices.Clear();
        }
        private void ClearListboxes()
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
        }

        private void btnMonday_Click(object sender, EventArgs e)
        {
            ClearListboxes();
            List<Shift> mondayShifts = scController.GetWeeklySchedule(0);
            DisplayAssignedShifts(mondayShifts, DayOfWeek.Monday, Time.MORNING, listBox1);
            DisplayAssignedShifts(mondayShifts, DayOfWeek.Monday, Time.AFTERNOON, listBox2);
            DisplayAssignedShifts(mondayShifts, DayOfWeek.Monday, Time.EVENING, listBox3);
        }

        private void btnTuesday_Click(object sender, EventArgs e)
        {
            ClearListboxes();
            List<Shift> tuesdayShifts = scController.GetWeeklySchedule(1);
            DisplayAssignedShifts(tuesdayShifts, DayOfWeek.Tuesday, Time.MORNING, listBox1);
            DisplayAssignedShifts(tuesdayShifts, DayOfWeek.Tuesday, Time.AFTERNOON, listBox2);
            DisplayAssignedShifts(tuesdayShifts, DayOfWeek.Tuesday, Time.EVENING, listBox3);
        }

        private void btnWednesday_Click(object sender, EventArgs e)
        {
            ClearListboxes();
            List<Shift> wednesdayShifts = scController.GetWeeklySchedule(2);
            DisplayAssignedShifts(wednesdayShifts, DayOfWeek.Wednesday, Time.MORNING, listBox1);
            DisplayAssignedShifts(wednesdayShifts, DayOfWeek.Wednesday, Time.AFTERNOON, listBox2);
            DisplayAssignedShifts(wednesdayShifts, DayOfWeek.Wednesday, Time.EVENING, listBox3);
        }

        private void btnThursdat_Click(object sender, EventArgs e)
        {
            ClearListboxes();
            List<Shift> thursdayShifts = scController.GetWeeklySchedule(3);
            DisplayAssignedShifts(thursdayShifts, DayOfWeek.Thursday, Time.MORNING, listBox1);
            DisplayAssignedShifts(thursdayShifts, DayOfWeek.Thursday, Time.AFTERNOON, listBox2);
            DisplayAssignedShifts(thursdayShifts, DayOfWeek.Thursday, Time.EVENING, listBox3);
        }

        private void btnFriday_Click(object sender, EventArgs e)
        {
            ClearListboxes();
            List<Shift> fridayShifts = scController.GetWeeklySchedule(4);
            DisplayAssignedShifts(fridayShifts, DayOfWeek.Friday, Time.MORNING, listBox1);
            DisplayAssignedShifts(fridayShifts, DayOfWeek.Friday, Time.AFTERNOON, listBox2);
            DisplayAssignedShifts(fridayShifts, DayOfWeek.Friday, Time.EVENING, listBox3);
        }

        private void btnSaturday_Click(object sender, EventArgs e)
        {
            ClearListboxes();
            List<Shift> saturdayShifts = scController.GetWeeklySchedule(5);
            DisplayAssignedShifts(saturdayShifts, DayOfWeek.Saturday, Time.MORNING, listBox1);
            DisplayAssignedShifts(saturdayShifts, DayOfWeek.Saturday, Time.AFTERNOON, listBox2);
            DisplayAssignedShifts(saturdayShifts, DayOfWeek.Saturday, Time.EVENING, listBox3);
        }

        private void DisplayAssignedShifts(List<Shift> dayShifts, DayOfWeek dayOfWeek, Time timeOfDay, ListBox lb)
        {
            Stack<MyListBoxItem> absentUsers = new Stack<MyListBoxItem>();
            List<MyListBoxItem> lateUsers = new List<MyListBoxItem>();
            List<MyListBoxItem> usersOnTime = new List<MyListBoxItem>();
            Stack<MyListBoxItem> others = new Stack<MyListBoxItem>();
            List<string> departments = new List<string>();
            int currentHour = DateTime.Now.Hour;
            int currentMinutes = DateTime.Now.Minute;

            MyListBoxItem lateEmoji = new MyListBoxItem(Color.Yellow, "♦►");
            MyListBoxItem absentEmoji = new MyListBoxItem(Color.Red, "!!");
            MyListBoxItem onTimeEmoji = new MyListBoxItem(Color.Green, "☺");

            foreach (Shift shift in dayShifts.Where(x => x.Time == timeOfDay))
            {
                foreach (User user in shift.Users)
                {
                    if (DateTime.Now.DayOfWeek > dayOfWeek || (DateTime.Now.DayOfWeek == dayOfWeek && DateTime.Now.Hour > shift.OriginalShiftStart.Hour))
                    {
                        if (shift.EmployeeStart.Hour > shift.OriginalShiftStart.Hour)
                        {
                            MyListBoxItem late = new MyListBoxItem(Color.Orange, user.FirstName + " " + user.Dep.DptName);
                            lateUsers.Add(late);
                        }
                        else if (shift.EmployeeStart.Hour <= shift.OriginalShiftStart.Hour && shift.EmployeeStart != DateTime.MinValue)
                        {
                            MyListBoxItem onTime = new MyListBoxItem(Color.Green, user.FirstName + " " + user.Dep.DptName);
                            usersOnTime.Add(onTime);
                        }
                        else if (shift.EmployeeStart == DateTime.MinValue)
                        {
                            MyListBoxItem absent = new MyListBoxItem(Color.Red, user.FirstName + " " + user.Dep.DptName);
                            absentUsers.Push(absent);
                        }
                    }
                    else
                    {
                        //default black color 
                        MyListBoxItem other = new MyListBoxItem(Color.Black, user.FirstName + " " + user.Dep.DptName);
                        others.Push(other);
                    }
                    departments.Add(user.Dep.DptName);
                }
            }

            List<string> allDepartments = dc.GetAllDepartments();
            //Check if there is a department without an employee for the shift
            if (dayShifts.Where(x => x.Time == timeOfDay).ToList().Count < allDepartments.Count)
            {
                //get the departments without an employee and put them in the list of absent
                foreach (string dep in stController.GetAllDepartments())
                {
                    if (!departments.Contains(dep) && dep != "ALL" && dep != "NONE")
                    {
                        MyListBoxItem emptyDepartment = new MyListBoxItem(Color.Red, dep);
                        absentUsers.Push(emptyDepartment);
                    }
                }
            }

            //when there are more than 10items in the listbox - there is scroll and it bugs!
            //foreach (string d in departments)
            //{
            //    listBox1.Items.Add(d);
            //}

            //display first the list with absent, then with late and finally with onTime employees
            while (absentUsers.Count > 0)
            {
                lb.Items.Add(absentUsers.Pop());
            }
            if (others.Count == 0)
            {
                foreach (MyListBoxItem item in lateUsers)
                {
                    lb.Items.Add(item);
                }
                foreach (MyListBoxItem item in usersOnTime)
                {
                    lb.Items.Add(item);
                }
            }
            else
            {
                foreach (MyListBoxItem item in others)
                {
                    lb.Items.Add(item);
                }
            }
        }
        private void SearchStock_OnTextChange_1(object sender, EventArgs e)
        {
            sc.GetStocks();
            lvStock.Items.Clear();
            ProductDepartment selectedDepartment = dc.GetDepartmentId(cmxCategoryStock.selectedIndex);
            foreach (var stock in sc.Stocks)
            {
                if (stock.Name.ToLower().Contains(SearchStock.text.ToLower()) || SearchStock.text == "Search by name")
                {
                    if (stock.Department == selectedDepartment)
                    {
                        string[] row = { stock.Id.ToString(), stock.Name, stock.Department.DptName.ToString(), stock.SupplyCost.ToString(), stock.Price.ToString(), stock.Quantity.ToString() };
                        lvStock.Items.Add(new ListViewItem(row));
                    }
                    else if (cmxCategoryStock.selectedIndex == -1)
                    {
                        string[] row = { stock.Id.ToString(), stock.Name, stock.Department.DptName.ToString(), stock.SupplyCost.ToString(), stock.Price.ToString(), stock.Quantity.ToString() };
                        lvStock.Items.Add(new ListViewItem(row));
                    }
                }
            }
        }

        private void SearchStock_Enter_1(object sender, EventArgs e)
        {
            SearchStock.text = "";
        }

        private void SearchStock_Leave_1(object sender, EventArgs e)
        {
            SearchStock.text = "Search by name";
        }

        private void lvStock_MouseClick_1(object sender, MouseEventArgs e)
        {
            ListView.SelectedIndexCollection indices = lvStock.SelectedIndices;

            if (indices.Count > 0)
            {
                var item = lvStock.SelectedItems[0];
                stockID = Convert.ToInt32(item.SubItems[0].Text);
            }
            indices.Clear();
        }

        private void cmxCategoryStock_onItemSelected(object sender, EventArgs e)
        {
            sc.GetStocks();
            lvStock.Items.Clear();
            int departmentId = dc.GetDepartment(cmxCategoryStock.selectedValue).Id;
            foreach (var stock in sc.Stocks)
            {
                if (stock.Name.ToLower().Contains(SearchStock.text.ToLower()) || SearchStock.text == "Search by name")
                {
                    if (stock.Department.Id == departmentId)
                    {
                        string[] row = { stock.Id.ToString(), stock.Name, stock.Department.DptName.ToString(), stock.SupplyCost.ToString(), stock.Price.ToString(), stock.Quantity.ToString() };
                        lvStock.Items.Add(new ListViewItem(row));
                    }
                    else if (cmxCategoryStock.selectedIndex == -1)
                    {
                        string[] row = { stock.Id.ToString(), stock.Name, stock.Department.DptName.ToString(), stock.SupplyCost.ToString(), stock.Price.ToString(), stock.Quantity.ToString() };
                        lvStock.Items.Add(new ListViewItem(row));
                    }
                }
            }
        }

        private void btnRequestStock_Click(object sender, EventArgs e)
        {
            if (stockID == 0)
                CustomMessageBoxController.ShowMessage("Choose a stock to edit first!", MessageBoxButtons.OK);
            else
            {
                sc.RequestStock(stockID);
                CustomMessageBoxController.ShowMessage("Stock successfully requested!", MessageBoxButtons.OK);
            }
        }

        private void btnOpenDepartmMenu_Click(object sender, EventArgs e)
        {
            dc = new DepartmentController();
            tabControlManager.SelectTab(tpDepartment);
            lbxAllDepartments.Items.Clear();
            lbxAllDepartments.Items.AddRange(dc.GetAllDepartments().ToArray());
        }

        private void lblDeleteDept_MouseClick(object sender, MouseEventArgs e)
        {
            pnlDeleteDept.Visible = true;
            cmxDeleteDepartment.Items = dc.GetAllDepartments().ToArray();
            cmxDeleteDepartment.selectedIndex = -1;
        }

        private void lblEditDept_MouseClick(object sender, MouseEventArgs e)
        {
            pnlEditDept.Visible = true;
            cmxDepartmentsEdit.Items = dc.GetAllDepartments().ToArray();
            cmxDepartmentsEdit.selectedIndex = -1;
            tbEditNameDept.Text = "";
        }

        private void lblAddNewDept_MouseClick(object sender, MouseEventArgs e)
        {
            pnlAddDepartment.Visible = true;
            tbAddDepartment.Text = "";
        }

        private void btnCloseDeleteDpt_Click(object sender, EventArgs e)
        {
            pnlDeleteDept.Visible = false;
        }

        private void btnCloseEditDpt_Click(object sender, EventArgs e)
        {
            pnlEditDept.Visible = false;
        }

        private void btnCloseAddDpt_Click(object sender, EventArgs e)
        {
            pnlAddDepartment.Visible = false;
        }

        private void btnSaveNewDepartment_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(tbAddDepartment.Text))
            {
                if (dc.AddDepartment(tbAddDepartment.Text))
                {
                    CustomMessageBoxController.ShowMessage("New department is added successfully!", MessageBoxButtons.OK);
                    pnlAddDepartment.Visible = false;
                    lbxAllDepartments.Items.Add(tbAddDepartment.Text);
                }
                else
                {
                    CustomMessageBoxController.ShowMessage("A department with this name already exists.", MessageBoxButtons.OK);
                    tbAddDepartment.Text = "";
                }
            }
            else
            {
                CustomMessageBoxController.ShowMessage("Please fill out the new name of the department", MessageBoxButtons.OK);
            }
        }

        private void btnSaveEditDepartment_Click(object sender, EventArgs e)
        {
            if(!String.IsNullOrWhiteSpace(tbEditNameDept.Text) && cmxDepartmentsEdit.selectedIndex != -1)
            {
                if (dc.EditDepartment(cmxDepartmentsEdit.selectedValue, tbEditNameDept.Text))
                {
                    CustomMessageBoxController.ShowMessage("The department is updated successfully!", MessageBoxButtons.OK);
                    pnlEditDept.Visible = false;
                    lbxAllDepartments.Items.Clear();
                    lbxAllDepartments.Items.AddRange(dc.GetAllDepartments().ToArray());
                }
                else
                {
                    CustomMessageBoxController.ShowMessage("The update was unsuccessful.", MessageBoxButtons.OK);
                }
            }
            else
            {
                CustomMessageBoxController.ShowMessage("Please fill out all fields.", MessageBoxButtons.OK);
            }
        }

        private void btnDeleteDept_Click(object sender, EventArgs e)
        {
            if(cmxDeleteDepartment.selectedIndex != -1)
            {
                if (dc.DeleteDepartment(cmxDeleteDepartment.selectedValue))
                {
                    CustomMessageBoxController.ShowMessage("The department was deleted successfully!", MessageBoxButtons.OK);
                    pnlDeleteDept.Visible = false;
                    lbxAllDepartments.Items.Remove(cmxDeleteDepartment.selectedValue);
                }
                else
                {
                    CustomMessageBoxController.ShowMessage("The delete was unsuccessful.", MessageBoxButtons.OK);
                }
            }
            else
            {
                CustomMessageBoxController.ShowMessage("Please select a department to delete.", MessageBoxButtons.OK);
            }
        }

        private void DrawListboxItem(object sender, DrawItemEventArgs e, ListBox lb)
        {
            MyListBoxItem item = lb.Items[e.Index] as MyListBoxItem; // Get the current item and cast it to MyListBoxItem
            if (item != null)
            {
                e.Graphics.DrawString( // Draw the appropriate text in the ListBox
                    item.Message, // The message linked to the item
                    lb.Font, // Take the font from the listbox
                    new SolidBrush(item.ItemColor), // Set the color 
                    0, // X pixel coordinate
                    e.Index * lb.ItemHeight // Y pixel coordinate.  Multiply the index by the ItemHeight defined in the listbox.
                );
            }
        }

        private void lvEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lblDeleteDept_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            DrawListboxItem(sender, e, listBox1);
        }

        private void listBox2_DrawItem(object sender, DrawItemEventArgs e)
        {
            DrawListboxItem(sender, e, listBox2);
        }

        private void listBox3_DrawItem(object sender, DrawItemEventArgs e)
        {
            DrawListboxItem(sender, e, listBox3);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pnlAddShift_Paint(object sender, PaintEventArgs e)
        {

        }
    }

    public class MyListBoxItem
    {
        public MyListBoxItem(Color c, string m)
        {
            ItemColor = c;
            Message = m;
        }
        public Color ItemColor { get; set; }
        public string Message { get; set; }
    }
}