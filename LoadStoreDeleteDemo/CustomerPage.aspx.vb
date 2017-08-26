Imports Persistence

Public Class CustomerPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ShowGrid()
        Catch ex As Exception
            lblMessage.Text = ex.Message
        End Try
    End Sub

    Private Sub ShowGrid()
        Dim list As New Customers()
        Database.LoadAll(Of Customer)(list, "")
        gvwCustomers.DataSource = list
        gvwCustomers.DataBind()
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim c As New Customer()

        If txtID.Text.IsNumeric() Then
            c.ID = CInt(txtID.Text)
        End If

        c.Name = txtName.Text
        c.AddressLine1 = txtAddressLine1.Text
        c.AddressLine2 = txtAddressLine2.Text
        c.City = txtCity.Text
        c.State = txtState.Text
        c.Zip = txtZip.Text

        Database.Store(c)
        txtID.Text = c.ID

        ShowGrid()
    End Sub

    Protected Sub gvwCustomers_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvwCustomers.SelectedIndexChanged
        Try
            Dim c As New Customer()
            If gvwCustomers.SelectedRow.Cells(1).Text.IsNumeric() Then
                c.ID = CInt(gvwCustomers.SelectedRow.Cells(1).Text)
            End If

            Database.Load(c)

            txtID.Text = c.ID
            txtName.Text = c.Name
            txtAddressLine1.Text = c.AddressLine1
            txtAddressLine2.Text = c.AddressLine2
            txtCity.Text = c.City
            txtState.Text = c.State
            txtZip.Text = c.Zip
        Catch ex As Exception
            lblMessage.Text = ex.Message
        End Try
    End Sub
End Class
