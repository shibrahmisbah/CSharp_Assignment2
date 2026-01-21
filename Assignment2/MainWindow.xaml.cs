using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using A2ShibrahMisbah.NorthwindDataSetTableAdapters;

namespace A2ShibrahMisbah
{
     
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //adapters - 3
        private ProductsWithCategoryNameAdp adpProductsWithCatName;
        private CategoriesTableAdapter adpCategories;
        private ProductsTableAdapter productsTableAdapter;

        //datatables - 3
        private NorthwindDataSet.ProductsWithCategoryNameDataTable tblProductsWithCategoryNames;
        private NorthwindDataSet.CategoriesDataTable tblCategories;
        private NorthwindDataSet.ProductsDataTable tblProducts; 



        public MainWindow()
        {
            InitializeComponent();

            adpCategories = new CategoriesTableAdapter();
            adpProductsWithCatName = new ProductsWithCategoryNameAdp();
            productsTableAdapter = new ProductsTableAdapter();

            tblProductsWithCategoryNames = new NorthwindDataSet.ProductsWithCategoryNameDataTable();
            tblCategories = new NorthwindDataSet.CategoriesDataTable();
            tblProducts = new NorthwindDataSet.ProductsDataTable();

            //GetAllProducts();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tblCategories = adpCategories.GetCategories();

            cmbCategories.ItemsSource = tblCategories;
            cmbCategories.DisplayMemberPath = "CategoryName";
            cmbCategories.SelectedValuePath = "CategoryID";

        }

        private void GetAllProducts()
        {

            tblProductsWithCategoryNames = adpProductsWithCatName.GetProductsByCategoryId();
            grdProducts.ItemsSource = tblProductsWithCategoryNames;


        }

        private void btnLoadAllProducts_Click(object sender, RoutedEventArgs e)
        {
            GetAllProducts();


        }

        private void clearData()
        {
            txtName.Text = txtId.Text = txtPrice.Text = txtQuantity.Text = "";
            GetAllProducts();

            //cmbCategories.SelectedIndex = -1;

            //activating 
            cmbCategories.SelectionChanged -= cmbCategories_SelectionChanged;
            //return to empty
            cmbCategories.SelectedValue = -1;
            //deactivating
            cmbCategories.SelectionChanged += cmbCategories_SelectionChanged;
        }

        private void btnClearData_Click(object sender, RoutedEventArgs e)
        {
            clearData();

        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {

            //check if strings are empty
            if(String.IsNullOrEmpty(txtName.Text) || String.IsNullOrEmpty(txtPrice.Text)
                    || String.IsNullOrEmpty(txtQuantity.Text) || String.IsNullOrEmpty(cmbCategories.Text))
            {
                MessageBox.Show("One or more fields are missing.");
            }
            else
            {
                string prodName = txtName.Text;
                decimal price = Convert.ToDecimal(txtPrice.Text);
                short quantity = Convert.ToInt16(txtQuantity.Text);
                int categoryID = (int)cmbCategories.SelectedValue;

                productsTableAdapter.Insert(prodName, price, quantity, categoryID);



                //clear all fields and refresh data grid
                clearData();
                MessageBox.Show("New product added!");

            }

           
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            //read id from textbox
            int id = int.Parse(txtId.Text);

            var row = tblProductsWithCategoryNames.FindByProductID(id);

            if(row != null)
            {
                txtName.Text = row.ProductName;
                txtPrice.Text = row.UnitPrice.ToString();
                txtQuantity.Text = row.UnitsInStock.ToString();
                cmbCategories.Text = row.CategoryName.ToString();
            }
            else
            {
                //clear fields, print message
                txtName.Text = txtPrice.Text = txtQuantity.Text = "";
                MessageBox.Show("Invalid product ID. Please try again.");
                txtId.Focus();
            }


        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

            tblProductsWithCategoryNames = adpProductsWithCatName.GetProductsByProdName(txtName.Text);
            grdProducts.ItemsSource = tblProductsWithCategoryNames;
        }

        private void cmbCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int categoryID = (int)cmbCategories.SelectedValue;

            //call the query
            tblProductsWithCategoryNames = adpProductsWithCatName.GetProductsBySelectedCategory(categoryID);

            //refresh data grid
            grdProducts.ItemsSource = tblProductsWithCategoryNames;

        }
    }
}
