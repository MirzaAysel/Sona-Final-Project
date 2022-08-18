const searchBtns = document.querySelectorAll(".search-btn");
const searchModal = document.querySelector(".search-model");

const searchClose = document.querySelector(".search-close-switch");

searchBtns.forEach((searchBtn) => {
  searchBtn.addEventListener("click", function () {
    searchModal.style.display = "block";
  });
});

searchClose.addEventListener("click", function () {
  searchModal.style.display = "none";
});
