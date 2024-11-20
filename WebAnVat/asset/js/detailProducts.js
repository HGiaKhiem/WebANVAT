// Trang chi tiết sản phẩm: tăng giảm sản phẩm
const counters = document.querySelectorAll(".counter")
counters.forEach(counter => {
    let number = 0
    const numberDisplay = counter.querySelector(".number")
    const btnMinus = counter.querySelector(".btnMinus")
    const btnPlus = counter.querySelector(".btnPlus")

    btnMinus.addEventListener("click", () => {
        number--
        if (number < 0) {
            number = 0
        }
        updateDisplay()
    })

    btnPlus.addEventListener("click", () => {
        number++
        updateDisplay()
    })

    function updateDisplay() {
        numberDisplay.textContent = number
    }
})

//Size
const btnSizes = document.querySelectorAll(".wrapDetail__btnSize")
btnSizes[0].classList.add('active')
btnSizes.forEach((btn, index) => {
    // lắng nghe sự kiện khi click vào nút Size nào thì nút Size đó được thêm class active đồng thời xóa nút Size cũ có class active
    btn.addEventListener('click', () => {
        document.querySelector(".wrapDetail__btnSize.active").classList.remove('active')
        btn.classList.add('active')
    })
})

//thêm class active vào những option(vd: Ngọt(ít, bth, nhiều)) khi có class active thì sẽ thay đổi màu sắc
const options = document.querySelectorAll(".wrapDetail__wrapOption")
options.forEach((option, index) => {
    const btnOptions = option.querySelectorAll(".wrapDetail__btn")
    btnOptions.forEach((btn, index) => {
        btn.addEventListener("click", () => {
            option.querySelector(".wrapDetail__btn.active").classList.remove('active')
            btn.classList.add('active')
        })
    })
})





