var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#myTable').DataTable({
        "ajax": {
            url: '/admin/product/GetAll'
        },
        "columns": [
            { data: 'name', "width": '35%' },
            { data: 'author', "width": '15%' },
            { data: 'listPrice', "width": '5%' },
            { data: 'category.name', "width": '10%' },
            {
                data: 'id', "width": '35%', render: function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2">
                            <i class="bi bi-pencil-square"></i> Edit
                        </a>
                        <a onClick=deleteProduct('${data}') class="btn btn-danger mx-2">
                            <i class="bi bi-trash3-fill"></i> Delete
                        </a>
                    </div>`;
                }
            },
        ]
    });
}

function deleteProduct(id) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "/admin/product/DeleteProduct?id=" + id,
                type: 'DELETE',
                success: function (data) {
                    Swal.fire({
                        title: "Deleted!",
                        text: "The product has been deleted.",
                        icon: "success"
                    });
                    dataTable.ajax.reload();
                }
            })   
        }
    });
};
