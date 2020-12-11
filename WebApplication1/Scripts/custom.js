const addButton = document.querySelector('#add_ingredients')
var input = document.querySelector('.search-query')
const container = document.querySelector('.ingredients')
var includeIngredients = []

class item {
    constructor(itemName) {
        this.createDiv(itemName)
    }
    createDiv(itemName) {
        let input = document.createElement('span')
        input.innerHTML = itemName
        // input.disabled = false
        input.classList.add('item_input')
        // input.type = "text"
        includeIngredients.push(itemName)
        let itemBox = document.createElement('li')
        itemBox.classList.add('item')

        let removeButton = document.createElement('button')
        removeButton.innerHTML = "<i class='fa fa-times'></i>"
        removeButton.classList.add('removeButton')

        container.appendChild(itemBox)

        itemBox.appendChild(input)
        itemBox.appendChild(removeButton)

        removeButton.addEventListener('click', () => this.remove(itemBox))
    }

    edit(input) {
        input.disabled = !input.disabled
    }

    remove(item) {
        container.removeChild(item)
    }
}

function check() {
    if (input.value != "") {
        new item(input.value)
        input.value = ""
    }
}

addButton.addEventListener('click', check)

window.addEventListener('keydown', (e) => {
    if (e.which == 13) {
        check();
    }
})



/*

const registerDiv = document.getElementById("register")
const registerLink = document.getElementById("registerLink")
const modalDialog = document.getElementById("modal-dialog")

registerLink.addEventListener("click", (e) => {
    e.preventDefault()
    modalDialog.style.marginTop = "102px"
    registerDiv.classList.add("show")
    registerDiv.style.display = "block"
    registerDiv.setAttribute("aria-modal", true)
    registerDiv.setAttribute("role", "dialog")
    registerDiv.removeAttribute("aria-hidden")
    document.body.classList.add("modal-open")
})


*/