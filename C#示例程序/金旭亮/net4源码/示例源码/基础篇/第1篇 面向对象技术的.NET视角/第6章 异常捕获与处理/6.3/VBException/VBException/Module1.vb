Module Module1
    Sub Main()
        Dim ShouldCatch As Boolean = True
        Try
            Dim number As Integer = Convert.ToInt32(Console.ReadLine())
        Catch ex As FormatException When ShouldCatch
            Console.WriteLine(ex.Message)
        End Try
    End Sub

End Module
