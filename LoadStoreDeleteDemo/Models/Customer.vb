Imports Persistence

Public Class Customers
    Inherits PersistentList(Of Customer)

End Class

<Persistable("Customer", "ID")>
Public Class Customer

    'constructors
    Public Sub Customer()

    End Sub

    Public Sub Customer(ID As Integer)
        ID = ID
        Database.Load(Me)
    End Sub

    'persistent properties
    Public Property ID As Integer
    Public Property Name As String
    Public Property AddressLine1 As String
    Public Property AddressLine2 As String
    Public Property City As String
    Public Property State As String
    Public Property Zip As String

    'transient properties

End Class
