using MediaBazarProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaBazarProject
{
    //TODO: CheckEmptyListView();
    public partial class FormAdmin : Form
    {
        StatisticsController stController;
        UserController uc;
        StockController sc;
        RequestsController rc;
        DepartmentController dc;

        User currentUser;
        int loggedUserId;
        User userToEdit;
        Stock stockToEdit;
        int currentID = 0;
        int stockID = 0;
        //int departmentID;
        string departmentName;
        DateTime dOb;
        DateTime dOs;
        Role role;
        Gender gender;
        Role selectedRole;
        private List<Control> allLabels;
        private List<int> displayedRequestsIDs;

        public FormAdmin(Form form, int id)
        {
            InitializeComponent();

            stController = new StatisticsController();
            lblTotalIncome.Text = stController.GetTotalIncome().Values.First().ToString() + "$";
            lbTotprNumber.Text = stController.GetTotalProducts().ToString();
            lbltotEmplNumber.Text = stController.GetTotalUsers().ToString();

            uc = new UserController();
            loggedUserId = id;
            sc = new StockController();
            currentID = id;
            dc = new DepartmentController();

            rc = new RequestsController();
            allLabels = new List<Control>();
            displayedRequestsIDs = new List<int>();

        }

        // Form Load
        //Assign labels to current User
        private void FormAdmin_Load(object sender, EventArgs e)
        {
            uc.ExtractAllUsers();
            currentUser = uc.GetUser(loggedUserId);
            lblUserName.Text = currentUser.Username;
            bunifuCustomLabel2.Text = "Welcome " + currentUser.FirstName;
            bunifuCustomLabel1.Text = "You are logged in as " + Environment.NewLine + currentUser.Role;
            UpdateListView();
            RefillDropDownLists();
            datePicker.Value = DateTime.Now;
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

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    Form formmanager = new FormManager();
        //    formmanager.Show();
        //}

        bool activeBtnRestock = false;
        bool activeBtnRequest = false;

        private void btnRequests_Click(object sender, EventArgs e)
        {
            if (!activeBtnRestock)
            {
                pnlRestock.BringToFront();
                activeBtnRestock = true;
                LoadRequests();
            }
            else
            {
                pnlRestock.SendToBack();
                activeBtnRestock = false;
            }

        }

        private void ClearShiftRequestsLabels()
        {
            for (int i = 0; i < 5; i++)
            {
                GetNameLabel(i + 1).Text = "";
                GetRequestLabel(i + 1).Text = "";
            }
        }

        private void LoadRequests()
        {
            List<ShiftRequest> top5 = new List<ShiftRequest>();
            if (counterViewMore > 0)
            {
                top5 = rc.LoadShiftRequests().Skip(counterViewMore * 5).Take(5).ToList();
                if (top5.Count == 0)
                {
                    counterViewMore--;
                    CustomMessageBoxController.ShowMessage("No more requests!", MessageBoxButtons.OK);
                    return;
                }
            }
            else if (counterViewMore < 0)
            {
                counterViewMore++;
                CustomMessageBoxController.ShowMessage("No previous pages!", MessageBoxButtons.OK);
                return;
            }
            else
            { top5 = rc.LoadShiftRequests().Take(5).ToList(); }

            if (top5.Count > 0)
            {
                linkLabel1.Visible = true;
                linkLabel2.Visible = true;
                GetAllLabels();
                ClearShiftRequestsLabels();
                displayedRequestsIDs.Clear();
                for (int i = 0; i < top5.Count; i++)
                {
                    displayedRequestsIDs.Add(top5[i].RequestID);
                    GetNameLabel(i + 1).Text = top5[i].ShowIdName();
                    GetRequestLabel(i + 1).Text = top5[i].ToString();
                }
            }
            else
            {
                lblEmpIdNameReq1.Text = "No requests";
                linkLabel1.Visible = false;
                linkLabel2.Visible = false;
            }
        }

        private Label GetNameLabel(int n)
        {
            foreach (Label l in this.allLabels)
            {
                if (l.Name == $"lblEmpIdNameReq{n}")
                { return l; }
            }
            return null;
        }


        private void GetAllLabels()
        {
            allLabels = GetAllControls(this);
        }

        private List<Control> GetAllControls(Control container, List<Control> list)
        {
            foreach (Control c in container.Controls)
            {
                if (c is Label) list.Add(c);
                if (c.Controls.Count > 0)
                    list = GetAllControls(c, list);
            }
            return list;
        }
        private List<Control> GetAllControls(Control container)
        {
            return GetAllControls(container, new List<Control>());
        }

        private Label GetRequestLabel(int n)
        {
            foreach (Label l in this.allLabels)
            {
                if (l.Name == $"lblEmpReq{n}")
                { return l; }
            }
            return null;
        }

        private void btnChangeAuthorisation_Click(object sender, EventArgs e)
        {
            if (!activeBtnRequest)
            {
                pnlEmplRequest.BringToFront();
                activeBtnRequest = true;
                LoadRequests();
            }
            else
            {
                pnlEmplRequest.SendToBack();
                activeBtnRequest = false;
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            tabControlAdmin.SelectTab(tpHome);
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnStocksMenu_Click(object sender, EventArgs e)
        {
            tabControlAdmin.SelectTab(tpStocks);
            lvStock.HideSelection = true;
            panelAddProduct.Visible = false;
        }

        private void btnEmployeeMenu_Click(object sender, EventArgs e)
        {
            tabControlAdmin.SelectTab(tpEmployees);
            currentID = 0;

            uc.ExtractAllUsers();
            UpdateListView();
            if (lvEmployee.Items.Count == 0)
            {
                CustomMessageBoxController.ShowMessage("No employees found!", MessageBoxButtons.OK);
            }
            if (searchBar.text != "Search by name")
            {
                searchBar.text = "Search by name";
            }

            searchBar.Enabled = true;
            panelAddUser.Visible = false;
            lvEmployee.Enabled = true;
            lvEmployee.HideSelection = true;
        }

        private void btnConfirmAdd_Click(object sender, EventArgs e)
        {
            if (BlankFieldsExist())
            {
                CustomMessageBoxController.ShowMessage("All fields are obligatory!", MessageBoxButtons.OK);
            }
            else
            {
                if (UsernameExists())
                {
                    CustomMessageBoxController.ShowMessage("This username already exists! Update fail!", MessageBoxButtons.OK);
                }
                else
                {
                    PrepareValues();
                    if (uc.AddNewUser(tbAddFirstName.Text, tbAddLastName.Text, dOb, tbAddAddress.Text, gender, tbAddPhone.Text, dOs, tbAddUsername.Text, tbAddPassword.Text, tbAddEmail.Text, Convert.ToDouble(tbAddSalary.Text), role, 1, departmentName))
                    {
                        int id = uc.lastInsertedUserId;
                        UpdateListView();
                        try
                        {
                            if (selectedRole == Role.EMPLOYEE)
                            { uc.AddUserAvailability(id, switchMonday.Value, switchTuesday.Value, switchWednesday.Value, switchThursday.Value, switchFriday.Value, switchSaturday.Value); }
                        }
                        catch (Exception ex)
                        { CustomMessageBoxController.ShowMessage(ex.Message, MessageBoxButtons.OK); }
                        //uc.AddUserPreferences(id, switchMonday.Value, switchTuesday.Value, switchWednesday.Value, switchThursday.Value, switchFriday.Value, switchSaturday.Value);
                        CustomMessageBoxController.ShowMessage("New user successfully added!", MessageBoxButtons.OK);
                        panelAddUser.Visible = false;
                    }
                    else
                    {
                        CustomMessageBoxController.ShowMessage("Sorry, could not add user!", MessageBoxButtons.OK);
                    }
                }
                dropdownAddDepartment.Enabled = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            panelAddUser.Visible = true;
            lblCurrPage.Text = "Add user";
            btnUpdate.Visible = false;
            btnReset.Visible = true;
            btnConfirmAdd.Visible = true;
            btnClosePopUpPanel.Visible = false;
            ClearFields();
            EnableFieldInput();
            RefillDropDownLists();
            SwitchOffAvailability();
            DisableAvailabilitySwitchers();
        }

        private void HideAddProduct()
        {
            panelAddProduct.Visible = false;
            btnReset.Visible = false;
            btnConfirmAdd.Visible = false;
            btnUpdate.Visible = true;

        }


        private void btnAddStock_Click(object sender, EventArgs e)
        {
            panelAddProduct.Visible = true;
            btnReset.Visible = true;
            btnConfirmAdd.Visible = true;
            btnUpdate.Visible = false;
            ResetAddProductInput();


            /////////////////////////////////////////////////////////////////////////////////////////////////////
        }

        private void searchBar_Enter(object sender, EventArgs e)
        {
            searchBar.text = "";
        }

        private void searchBar_OnTextChange(object sender, EventArgs e)
        {
            if (searchBar.text != "Search by name")
            {
                lvEmployee.Items.Clear();
                foreach (User u in uc.GetUsers(searchBar.text))
                {
                    string names = u.FirstName + " " + u.LastName;
                    string department = "-";
                    if (!(u is Admin))
                    {
                        //department = Convert.ToString(u.Department);
                        department = u.Dep.DptName;
                    }
                    string[] row = { Convert.ToString(u.Id), names, department, Convert.ToString(u.Salary), u.Email };
                    ListViewItem item = new ListViewItem(row);
                    lvEmployee.Items.Add(item);
                }
            }
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            ClearFields();

            //Availability:
            SwitchOffAvailability();
            DisableAvailabilitySwitchers();
        }

        private void dropdownAddRole_onItemSelected(object sender, EventArgs e)
        {
            SwitchOffAvailability();
            DisableAvailabilitySwitchers();
            if ((Role)dropdownAddRole.selectedIndex == Role.EMPLOYEE)
            {
                if (currentID > 0 && uc.GetUser(currentID).Role == Role.EMPLOYEE)
                { LoadAvailability(); }
                else
                { SwitchOnAvailability(); }
                EnableAvailabilitySwitchers();
            }
            if ((Role)dropdownAddRole.selectedIndex == Role.ADMINISTRATOR)
            {
                dropdownAddDepartment.selectedIndex = Convert.ToInt32(Department.NONE);
                dropdownAddDepartment.Enabled = false;
            }
            else
            {
                dropdownAddDepartment.selectedIndex = -1;
                dropdownAddDepartment.Enabled = true;
            }
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (currentID != 0)
            {
                if (currentID != loggedUserId)
                {
                    CustomMessageBoxController.ShowMessage($"Deleted user: {uc.DeleteUser(currentID)}", MessageBoxButtons.OK);
                    UpdateListView();
                }
                else
                {
                    CustomMessageBoxController.ShowMessage("You cannot delete yourself, please log in from different account to proceed.", MessageBoxButtons.OK);
                }
            }
            else { CustomMessageBoxController.ShowMessage("Choose a user to delete first!", MessageBoxButtons.OK); }
        }

        private void LoadDepartmentsInDropDown()
        {
            //Load departments in combobox
            dropdownAddDepartment.Clear();
            foreach (string dep in dc.GetAllDepartments())
            {
                dropdownAddDepartment.AddItem(dep);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (currentID != 0)
            {
                panelAddUser.Visible = true;
                lblCurrPage.Text = "Edit user";
                btnReset.Visible = false;
                btnConfirmAdd.Visible = false;
                btnUpdate.Visible = true;
                btnClosePopUpPanel.Visible = true;
                //LoadDepartmentsInDropDown();
                RefillDropDownLists();
                EnableFieldInput();
                DisplayUserInfo();

                //Availability:
                if (userToEdit.Role == Role.EMPLOYEE)
                {
                    LoadAvailability();
                    EnableAvailabilitySwitchers();
                }
                else
                {
                    SwitchOffAvailability();
                    DisableAvailabilitySwitchers();
                }
            }
            else
            {
                CustomMessageBoxController.ShowMessage("Choose a user to edit first!", MessageBoxButtons.OK);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (BlankFieldsExist())
            {
                CustomMessageBoxController.ShowMessage("All fields are obligatory!", MessageBoxButtons.OK);
            }
            else
            {
                if (UsernameExists())
                {
                    CustomMessageBoxController.ShowMessage("This username already exists!", MessageBoxButtons.OK);
                }
                else
                {
                    PrepareValues();
                    Role defaultRole = uc.GetUser(currentID).Role;
                    if (uc.EditUser(currentID, tbAddFirstName.Text, tbAddLastName.Text, dOb, tbAddAddress.Text, gender, tbAddPhone.Text, dOs, tbAddUsername.Text, tbAddPassword.Text,
                        tbAddEmail.Text, Convert.ToDouble(tbAddSalary.Text), role, 1, departmentName))
                    {
                        try
                        {
                            if (defaultRole == selectedRole && selectedRole == Role.EMPLOYEE)
                            {
                                uc.EditUserAvailability(currentID, switchMonday.Value, switchTuesday.Value, switchWednesday.Value, switchThursday.Value, switchFriday.Value,
                                    switchSaturday.Value);
                            }
                            else if (defaultRole != selectedRole && defaultRole == Role.EMPLOYEE)
                            {
                                uc.DeleteUserAvailability(currentID);
                            }
                            else if (defaultRole != selectedRole && selectedRole == Role.EMPLOYEE)
                            {
                                uc.AddUserAvailability(currentID, switchMonday.Value, switchTuesday.Value, switchWednesday.Value, switchThursday.Value, switchFriday.Value,
                                    switchSaturday.Value);
                            }
                            CustomMessageBoxController.ShowMessage($"User with ID: { currentID} is successfully updated!", MessageBoxButtons.OK);
                        }
                        catch (Exception ex)
                        { CustomMessageBoxController.ShowMessage(ex.Message, MessageBoxButtons.OK); }
                    }
                    else
                    { CustomMessageBoxController.ShowMessage("Failed to update user!", MessageBoxButtons.OK); }
                }
            }
        }

        private void searchBar_Leave(object sender, EventArgs e)
        {
            searchBar.text = "Search by name";
        }

        private void lvEmployee_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (currentID != 0)
            {
                lblCurrPage.Text = $"View user information";
                RefillDropDownLists();
                DisplayUserInfo();
                DisableFieldInput();
                btnReset.Visible = false;
                btnConfirmAdd.Visible = false;
                btnUpdate.Visible = false;
                panelAddUser.Visible = true;
                //Availability:
                if (uc.GetUser(currentID).Role == Role.EMPLOYEE)
                {
                    try
                    {
                        LoadAvailability();
                    }
                    catch (Exception ex)
                    {
                        CustomMessageBoxController.ShowMessage(ex.Message, MessageBoxButtons.OK);
                    }
                }
                else { SwitchOffAvailability(); }
                DisableAvailabilitySwitchers();
            }
            else
            {
                CustomMessageBoxController.ShowMessage("Choose a user to edit first!", MessageBoxButtons.OK);
            }
        }

        //Additional methods for clearness 
        private bool UsernameExists()
        {
            uc.ExtractAllUsers();
            foreach (User u in uc.AllUsers)
            {
                if (u.Id != currentID && u.Username == tbAddUsername.Text)
                {
                    return true;
                }
            }
            return false;
        }
        private void DisplayUserInfo()
        {
            userToEdit = uc.GetUser(currentID);
            if (userToEdit != null)
            {
                tbAddFirstName.Text = userToEdit.FirstName;
                tbAddLastName.Text = userToEdit.LastName;
                tbAddUsername.Text = userToEdit.Username;
                tbAddPassword.Text = userToEdit.Password;
                tbAddEmail.Text = userToEdit.Email;
                tbAddAddress.Text = userToEdit.Address;
                tbAddPhone.Text = userToEdit.Phone;
                tbAddSalary.Text = userToEdit.Salary.ToString();

                dropdownAddGender.selectedIndex = Convert.ToInt32(userToEdit.Gender);
                dropdownAddRole.selectedIndex = Convert.ToInt32(userToEdit.Role);
                dropdownAddDepartment.selectedIndex = stController.GetAllDepartments().IndexOf(userToEdit.Dep.DptName);

                dropdownAddBirthDay.selectedIndex = userToEdit.DateOfBirth.Day - 1;
                dropdownAddBirthMonth.selectedIndex = userToEdit.DateOfBirth.Month - 1;
                dropdownAddBirthYear.selectedIndex = dropdownAddBirthYear.Items.ToList().IndexOf(userToEdit.DateOfBirth.Year.ToString());

                dropdownAddStartDay.selectedIndex = userToEdit.DateOfStart.Day - 1;
                dropdownAddStartMonth.selectedIndex = userToEdit.DateOfStart.Month - 1;
                dropdownAddStartYear.selectedIndex = dropdownAddStartYear.Items.ToList().IndexOf(userToEdit.DateOfStart.Year.ToString());
            }
        }

        private void LoadAvailability()
        {
            try
            {
                switchMonday.Value = uc.isAvailable(currentID, 0);
                switchTuesday.Value = uc.isAvailable(currentID, 1);
                switchWednesday.Value = uc.isAvailable(currentID, 2);
                switchThursday.Value = uc.isAvailable(currentID, 3);
                switchFriday.Value = uc.isAvailable(currentID, 4);
                switchSaturday.Value = uc.isAvailable(currentID, 5);
            }
            catch (Exception ex)
            {
                CustomMessageBoxController.ShowMessage(ex.Message, MessageBoxButtons.OK);
            }
        }

        private void DisableAvailabilitySwitchers()
        {
            switchMonday.Enabled = false;
            switchTuesday.Enabled = false;
            switchWednesday.Enabled = false;
            switchThursday.Enabled = false;
            switchFriday.Enabled = false;
            switchSaturday.Enabled = false;
        }

        private void EnableAvailabilitySwitchers()
        {
            switchMonday.Enabled = true;
            switchTuesday.Enabled = true;
            switchWednesday.Enabled = true;
            switchThursday.Enabled = true;
            switchFriday.Enabled = true;
            switchSaturday.Enabled = true;
        }
        private void SwitchOffAvailability()
        {
            switchMonday.Value = false;
            switchTuesday.Value = false;
            switchWednesday.Value = false;
            switchThursday.Value = false;
            switchFriday.Value = false;
            switchSaturday.Value = false;
        }

        private void SwitchOnAvailability()
        {
            switchMonday.Value = true;
            switchTuesday.Value = true;
            switchWednesday.Value = true;
            switchThursday.Value = true;
            switchFriday.Value = true;
            switchSaturday.Value = true;
        }
        private void UpdateListView()
        {
            lvEmployee.Items.Clear();
            lvStock.Items.Clear();
            lvStockRequests.Items.Clear();
            sc.GetStocks();
            sc.GetStockRequests();

            foreach (Stock stock in sc.Stocks)
            {
                string[] row = { stock.Id.ToString(), stock.Name, stock.Department.DptName.ToString(), stock.SupplyCost.ToString(), stock.Price.ToString(), stock.Quantity.ToString() };
                lvStock.Items.Add(new ListViewItem(row));
            }

            foreach (StockRequest stockRequest in sc.StockRequests)
            {
                string[] row;
                if (stockRequest.Completed.Equals(true))
                row = new string[]{ stockRequest.Id.ToString(), stockRequest.Name, stockRequest.Department.DptName.ToString(), stockRequest.DateOfRequest.ToString(), stockRequest.RestockQuantity.ToString(), "Closed" };
                else
                row = new string[] { stockRequest.Id.ToString(), stockRequest.Name, stockRequest.Department.DptName.ToString(), stockRequest.DateOfRequest.ToString(), stockRequest.RestockQuantity.ToString(), "Open" };

                lvStockRequests.Items.Add(new ListViewItem(row));
            }

            foreach (User u in uc.AllUsers)
            {
                string names = u.FirstName + " " + u.LastName;
                string department = "-";
                if (!(u is Admin))
                {
                    //department = Convert.ToString(u.Department);
                   department = u.Dep.DptName;
                }
                string[] row = { Convert.ToString(u.Id), names, department, Convert.ToString(u.Salary), u.Email };
                ListViewItem item = new ListViewItem(row);
                lvEmployee.Items.Add(item);
            }
        }
        private void PrepareValues()
        {
            //Prepare for database
            dOb = Convert.ToDateTime(dropdownAddBirthDay.selectedValue.Trim() + dropdownAddBirthMonth.selectedValue.Trim() + dropdownAddBirthYear.selectedValue.Trim());
            dOs = Convert.ToDateTime(dropdownAddStartDay.selectedValue.Trim() + dropdownAddStartMonth.selectedValue.Trim() + dropdownAddStartYear.selectedValue.Trim());
            role = (Role)(dropdownAddRole.selectedIndex + 1);
            gender = (Gender)(dropdownAddGender.selectedIndex + 1);
            departmentName = dropdownAddDepartment.selectedValue;

            //Get selected role:
            selectedRole = (Role)(dropdownAddRole.selectedIndex);
        }
        private bool BlankFieldsExist()
        {
            if (tbAddFirstName.Text == "" || tbAddLastName.Text == "" || tbAddUsername.Text == "" || tbAddPassword.Text == "" || tbAddEmail.Text == "" || tbAddAddress.Text == "" ||
                tbAddPhone.Text == "" || dropdownAddGender.selectedIndex == -1 || dropdownAddBirthDay.selectedIndex == -1 || dropdownAddBirthMonth.selectedIndex == -1 ||
                dropdownAddBirthYear.selectedIndex == -1 || dropdownAddStartDay.selectedIndex == -1 || dropdownAddStartMonth.selectedIndex == -1 || dropdownAddStartYear.selectedIndex == -1
                || tbAddSalary.Text == "" || dropdownAddRole.selectedIndex == -1)
            {
                return true;
            }
            return false;
        }
        private void DisableFieldInput()
        {
            tbAddFirstName.Enabled = false;
            tbAddLastName.Enabled = false;
            tbAddUsername.Enabled = false;
            tbAddPassword.Enabled = false;
            tbAddEmail.Enabled = false;
            tbAddAddress.Enabled = false;
            tbAddPhone.Enabled = false;
            tbAddSalary.Enabled = false;
        }

        private void EnableFieldInput()
        {
            tbAddFirstName.Enabled = true;
            tbAddLastName.Enabled = true;
            tbAddUsername.Enabled = true;
            tbAddPassword.Enabled = true;
            tbAddEmail.Enabled = true;
            tbAddAddress.Enabled = true;
            tbAddPhone.Enabled = true;
            tbAddSalary.Enabled = true;
            dropdownAddGender.Enabled = true;
            dropdownAddRole.Enabled = true;
            dropdownAddDepartment.Enabled = true;
            dropdownAddBirthDay.Enabled = true;
            dropdownAddBirthMonth.Enabled = true;
            dropdownAddBirthYear.Enabled = true;
            dropdownAddStartDay.Enabled = true;
            dropdownAddStartMonth.Enabled = true;
            dropdownAddStartYear.Enabled = true;
        }
        private void ClearFields()
        {
            tbAddFirstName.Text = "";
            tbAddLastName.Text = "";
            tbAddUsername.Text = "";
            tbAddPassword.Text = "";
            tbAddEmail.Text = "";
            tbAddAddress.Text = "";
            tbAddPhone.Text = "";
            tbAddSalary.Text = "";
            RefillDropDownLists();
            dropdownAddBirthDay.selectedIndex = 0;
            dropdownAddBirthMonth.selectedIndex = 0;
            dropdownAddBirthYear.selectedIndex = dropdownAddBirthYear.Items.ToList().IndexOf("2000");
            dropdownAddStartDay.selectedIndex = 0;
            dropdownAddStartMonth.selectedIndex = 0;
            dropdownAddStartYear.selectedIndex = dropdownAddStartYear.Items.ToList().IndexOf("2020");
        }

        private void RefillDropDownLists()
        {
            dropdownAddGender.Clear();
            foreach (Gender g in Enum.GetValues(typeof(Gender)))
            {
                dropdownAddGender.AddItem(g.ToString());
            }
            dropdownAddRole.Clear();
            foreach (Role r in Enum.GetValues(typeof(Role)))
            {
                dropdownAddRole.AddItem(r.ToString());
            }
            //dropdownAddDepartment.Clear();
            //foreach (Department d in Enum.GetValues(typeof(Department)))
            //{
            //    dropdownAddDepartment.AddItem(d.ToString());
            //}
            LoadDepartmentsInDropDown();
            ddDepartment.Clear();
            foreach (var item in stController.GetAllDepartments())
            {
                ddDepartment.AddItem(item);
            }
        }


        private void lvEmployee_DrawItem(object sender, DrawListViewItemEventArgs e)
        {

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

        private void CheckEmptyListView()
        {
            if (lvEmployee.Items.Count == 0)
            {
                UpdateListView();
            }
        }

        private void btnEditStock_Click(object sender, EventArgs e)
        {
            EditStockInput();
        }

        private void btnDeleteStock_Click(object sender, EventArgs e)
        {
            try
            {
                sc.DeleteStock(stockID);
                CustomMessageBoxController.ShowMessage("Stock is deleted!", MessageBoxButtons.OK);

            }
            catch (Exception)
            {
                CustomMessageBoxController.ShowMessage("Failed to delete stock!", MessageBoxButtons.OK);
            }
            UpdateListView();
        }

        private void searchBarStock_OnTextChange(object sender, EventArgs e)
        {
            lvStock.Items.Clear();
            foreach (var stock in sc.Stocks)
            {
                if (stock.Name.ToLower().Contains(searchBarStock.text.ToLower()))
                {
                    string[] row = { stock.Id.ToString(), stock.Name, stock.Department.DptName.ToString(), stock.SupplyCost.ToString(), stock.Price.ToString(), stock.Quantity.ToString() };
                    lvStock.Items.Add(new ListViewItem(row));
                }
            }
        }

        private void btnConfirmAddProduct_Click_1(object sender, EventArgs e)
        {
            panelAddProduct.Visible = false;

            try
            {
                if (btnConfirmAddProduct.ButtonText == "Add")
                {
                    if (sc.AddStock(tbName.Text, tbDescription.Text, Convert.ToDouble(tbTotalPrice.Text), Convert.ToDouble(tbSupplyCost.Text), datePicker.Value.Date,
                        Convert.ToInt32(tbQuantity.Text), Convert.ToInt32(tbTotalSales.Text), Convert.ToInt32(tbRestockQuantity.Text), swRequested.Value,
                        dc.GetDepartmentId(ddDepartment.selectedIndex + 1)))
                    {
                        CustomMessageBoxController.ShowMessage("New product successfully added!", MessageBoxButtons.OK);
                    }
                    else
                    {
                        CustomMessageBoxController.ShowMessage("Failed to add product!", MessageBoxButtons.OK);
                    }
                }
                else
                {
                    if (sc.EditStock(stockID, tbName.Text, tbDescription.Text, Convert.ToDouble(tbTotalPrice.Text), Convert.ToDouble(tbSupplyCost.Text), datePicker.Value.Date,
                        Convert.ToInt32(tbQuantity.Text), Convert.ToInt32(tbTotalSales.Text), Convert.ToInt32(tbRestockQuantity.Text), swRequested.Value,
                        dc.GetDepartmentId(ddDepartment.selectedIndex + 1)))
                    {
                        CustomMessageBoxController.ShowMessage("Product successfully edited!", MessageBoxButtons.OK);
                    }
                    else
                    {
                        CustomMessageBoxController.ShowMessage("Failed to edit product!", MessageBoxButtons.OK);
                    }
                }

            }
            catch (Exception)
            {
                CustomMessageBoxController.ShowMessage("Something went wrong.", MessageBoxButtons.OK);

            }
            finally
            {
                UpdateListView();
                ResetAddProductInput();
                HideAddProduct();
            }
        }

        private void btnResetForm_Click_1(object sender, EventArgs e)
        {
            if (btnConfirmAddProduct.ButtonText == "Add")
            {
                ResetAddProductInput();
            }
            else
            {
                EditStockInput();
            }
        }

        private void ResetAddProductInput()
        {
            tbName.ResetText();
            tbDescription.ResetText();
            tbTotalPrice.ResetText();
            tbSupplyCost.ResetText();
            tbQuantity.ResetText();
            tbTotalSales.ResetText();
            tbRestockQuantity.ResetText();
            datePicker.Value = DateTime.Now;
            swRequested.Value = false;
            ddDepartment.selectedIndex = 4;
            btnConfirmAddProduct.ButtonText = "Add";
            lblProduct.Text = "Add product";
        }

        private void searchBarStock_Enter(object sender, EventArgs e)
        {
            searchBarStock.text = "";
        }

        private void searchBarStock_Leave(object sender, EventArgs e)
        {
            searchBarStock.text = "Search by name";
        }

        private void lvStock_MouseClick(object sender, MouseEventArgs e)
        {
            ListView.SelectedIndexCollection indices = lvStock.SelectedIndices;

            if (indices.Count > 0)
            {
                var item = lvStock.SelectedItems[0];
                stockID = Convert.ToInt32(item.SubItems[0].Text);
            }
            indices.Clear();
        }
        private void EditStockInput()
        {
            try
            {
                stockToEdit = sc.GetStock(stockID);
                if (stockToEdit != null)
                {

                    lblProduct.Text = "Edit product";
                    btnConfirmAddProduct.ButtonText = "Edit";
                    panelAddProduct.Visible = true;
                    btnReset.Visible = true;
                    btnConfirmAdd.Visible = true;
                    btnUpdate.Visible = false;
                    btnClosePopUpPanel.Visible = true;

                    tbName.Text = stockToEdit.Name;
                    tbDescription.Text = stockToEdit.Description;
                    tbTotalPrice.Text = stockToEdit.Price.ToString();
                    tbSupplyCost.Text = stockToEdit.SupplyCost.ToString();
                    tbQuantity.Text = stockToEdit.Quantity.ToString();
                    tbTotalSales.Text = stockToEdit.TimesSold.ToString();
                    tbRestockQuantity.Text = stockToEdit.RestockQuantity.ToString();
                    datePicker.Value = stockToEdit.DateOfArrival;
                    swRequested.Value = stockToEdit.Requested;
                    ddDepartment.selectedIndex = stockToEdit.Department.Id - 1;
                }
            }
            catch (Exception)
            {

            }
        }

        private void lvStock_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (stockID != 0)
            {
                EditStockInput();
                RefillDropDownLists();
            }
        }

        private void dropdownAddRole_Click(object sender, EventArgs e)
        {

        }

        private void RejectRequest(ShiftRequest rejectedRequest)
        {
            if (rejectedRequest != null)
            {
                //DialogResult dialogResult = CustomMessageBoxController.ShowMessage("Are you sure you want to reject this request?", MessageBoxButtons.YesNo);
                if ((CustomMessageBoxController.ShowMessage("Are you sure you want to reject this request?", MessageBoxButtons.YesNo)) == DialogResult.Yes)
                {
                    bool isRejected = rc.Reject(rejectedRequest);
                    if (isRejected)
                    {
                        LoadRequests();
                        CustomMessageBoxController.ShowMessage("Request is rejected!", MessageBoxButtons.OK);
                    }
                    else
                    { CustomMessageBoxController.ShowMessage("Could not reject request!", MessageBoxButtons.OK); }
                }
            }
            else
            { CustomMessageBoxController.ShowMessage("There is no request to reject!", MessageBoxButtons.OK); }
        }

        private void btnRejectEmpReq1_Click(object sender, EventArgs e)
        {
            RejectRequest(rc.FindRequest(displayedRequestsIDs[0]));
        }

        private void btnRejectEmpReq2_Click(object sender, EventArgs e)
        {
            RejectRequest(rc.FindRequest(displayedRequestsIDs[1]));
        }

        private void btnRejectEmpReq3_Click(object sender, EventArgs e)
        {
            RejectRequest(rc.FindRequest(displayedRequestsIDs[2]));
        }

        private void btnRejectEmpReq4_Click(object sender, EventArgs e)
        {
            RejectRequest(rc.FindRequest(displayedRequestsIDs[3]));
        }

        private void btnRejectEmpReq5_Click(object sender, EventArgs e)
        {
            RejectRequest(rc.FindRequest(displayedRequestsIDs[4]));
        }

        private void ApproveRequest(ShiftRequest approvedRequest)
        {
            if (approvedRequest != null)
            {
                bool isApproved = rc.Approve(approvedRequest);
                if (isApproved)
                {
                    LoadRequests();
                    CustomMessageBoxController.ShowMessage("Request is approved!", MessageBoxButtons.OK);
                }
                else
                { CustomMessageBoxController.ShowMessage("Could not approve request!", MessageBoxButtons.OK); }
            }
            else
            { CustomMessageBoxController.ShowMessage("There is no request to approve!", MessageBoxButtons.OK); }
        }

        private void btnApproveEmpReq1_Click(object sender, EventArgs e)
        {
            ApproveRequest(rc.FindRequest(displayedRequestsIDs[0]));
        }

        private void btnApproveEmpReq2_Click(object sender, EventArgs e)
        {
            ApproveRequest(rc.FindRequest(displayedRequestsIDs[1]));
        }

        private void btnApproveEmpReq3_Click(object sender, EventArgs e)
        {
            ApproveRequest(rc.FindRequest(displayedRequestsIDs[2]));
        }

        private void btnApproveEmpReq4_Click(object sender, EventArgs e)
        {
            ApproveRequest(rc.FindRequest(displayedRequestsIDs[3]));
        }

        private void btnApproveEmpReq5_Click(object sender, EventArgs e)
        {
            ApproveRequest(rc.FindRequest(displayedRequestsIDs[4]));
        }

        int counterViewMore = 0;
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            counterViewMore++;
            LoadRequests();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            counterViewMore--;
            LoadRequests();
        }

        private void btnClosePopUpPanel_Click(object sender, EventArgs e)
        {
            panelAddUser.Visible = false;
        }

        private void btnCloseAddProducts_Click(object sender, EventArgs e)
        {
            panelAddProduct.Visible = false;
        }

        private void btnStockRequests_Click(object sender, EventArgs e)
        {
            tabControlAdmin.SelectTab(tpStockRequests);
            lvStockRequests.HideSelection = true;
        }
    }
}
