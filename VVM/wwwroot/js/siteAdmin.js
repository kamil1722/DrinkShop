function Delete(id) {
    const response = confirm("Вы уверены, что хотите удалить элемент?");

    if (response) {
        $.ajax({
            type: 'Delete',
            url: '/Admin/Delete',
            data: { itemId: id },
        })
        alert("Элемент успешно удалён");
    }
}
