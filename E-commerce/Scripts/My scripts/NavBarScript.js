function SearchProduct() {
    var searchBar = document.getElementById("searchBar");

    var searchValue = searchBar.value;

    if (searchValue === "")
        return;

    var url = '/Product/SearchProduct?name=' + searchValue;

    window.location.href = url;
}