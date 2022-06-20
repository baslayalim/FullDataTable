# Full Multi Function DataTable

Searching in all columns advanced features Using Select2, date and selection features

References
# Jquery DataTable
# Select2
# Dapper
# Mssql


# Sample Code

[HttpPost]
public JsonResult AdGetir(string ad)

{

 var model = _PublicConnectionStrings.Query<AutoAdArama>(@"select... ").ToList();
 
 return Json(model, JsonRequestBehavior.AllowGet);
 
}
 
  
return Json(new
 
{
 
 aaData = list,
 
 draw = _FormCollection.GetValues("draw")[0],
 
 recordsTotal = Total,
 
 recordsFiltered = Convert.ToInt32(Total)
 
 }, JsonRequestBehavior.AllowGet);
 
  
$(document).ready(function (a) {
 
 UsersList = $("#KullaniciListesi").dataTable({})
 
  
$.ajax({
 
 url: "/Persons/DurumGetir",
 
 type: "POST",
 
 dataType: "json",
 
 success: function (_post) {
 
 $.each(_post, function (i, item) {
 
  $('#FiltreDurum').append('<option value=' + item.value + '>' + item.description + ' </option>')
 
 });
 
 }
 
});
