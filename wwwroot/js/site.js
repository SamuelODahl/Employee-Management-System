

    function confirmDelete() {
        let result = confirm("Are you sure you want to delete?");
    if (result) {
        document.getElementById("deleteForm").submit();
        }
    }
