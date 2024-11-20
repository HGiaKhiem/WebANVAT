//------------------------------------ Trang chủ ------------------------------------------------
// tab UI modal-method
var tabs = document.querySelectorAll(".modal-method__item")
var panes = document.querySelectorAll(".modal-method__box")
var lines = document.querySelectorAll(".modal-method__line")

tabs.forEach((tab, index) => {
    const pane = panes[index]
    const line = lines[index]

    tab.addEventListener("click", (e) => {
        var currentTab = e.target
        document.querySelector(".modal-method__item.active").classList.remove("active")
        document.querySelector(".modal-method__box.active").classList.remove("active")
        document.querySelector(".modal-method__line.active").classList.remove("active")

        currentTab.classList.add("active")
        pane.classList.add("active")
        line.classList.add("active")
    })
})

// Hiển thị 4 sản phẩm trên 1 hàng nếu bấm vào nút xem tất cả thì hiển thị ALL
var wrapListFood = document.querySelector(".body__products")
var btnsViewAllFood = document.querySelector('#btn-products-viewAll-food')
var btnsViewAllDrink = document.querySelector('#btn-products-viewAll-drink')
var ProductsDrink = document.querySelectorAll("#products__list-drink .products__item")
var ProductsFood = document.querySelectorAll("#products__list-food .products__item")

function load3(arr) {
    var cd = arr.length
    for (var i = 0; i < cd; i++) {
        if (i < 4) {
            arr[i].classList.add("active")
        }
        else {
            break;
        }
    }
}

function loadAll(arr) {
    var cd = arr.length
    for (var i = 0; i < cd; i++) {
        arr[i].classList.add("active")
    }
}

btnsViewAllDrink.addEventListener('click', () => {
    loadAll(ProductsDrink)
    btnsViewAllDrink.style.display = 'none'
})

btnsViewAllFood.addEventListener('click', () => {
    loadAll(ProductsFood)
    btnsViewAllFood.style.display = 'none'
})

load3(ProductsFood)
load3(ProductsDrink)

// bắt sự kiện bấm phím enter để tìm kiếm
var btnSearch = document.querySelector(".navbar-left__search-icon")
var inputSearch = document.querySelector(".navbar-left__search-input")

inputSearch.addEventListener("keyup", (e) => {
    if (e.keyCode == '13') {
        btnSearch.onclick()
    }
})



