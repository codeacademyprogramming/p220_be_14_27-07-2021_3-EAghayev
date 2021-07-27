﻿$(document).ready(function () {

    $(document).on("click", ".show-book-modal", function (e) {
        e.preventDefault();

        var url = $(this).attr("href");
        fetch(url)
            .then(response => response.text())
            .then(data => {

                console.log(data)
                //var htmlStr = `<div class="row">
                //            <div class="col-lg-5">
                //                <!-- Product Details Slider Big Image-->
                //              <img class="poster"  src="/image/products/`+ data.bookImages[0].image + `"/>
                //                <!-- Product Details Slider Nav -->
                //            </div>
                //            <div class="col-lg-7 mt--30 mt-lg--30">
                //                <div class="product-details-info pl-lg--30 ">
                //                    <p class="tag-block">Tags: <a href="#">Movado</a>, <a href="#">Omega</a></p>
                //                    <h3 class="product-title">`+ data.name + `</h3>
                //                    <ul class="list-unstyled">
                //                        <li>Ex Tax: <span class="list-value"> £60.24</span></li>
                //                        <li>Brands: <a href="#" class="list-value font-weight-bold"> Canon</a></li>
                //                        <li>Product Code: <span class="list-value"> model1</span></li>
                //                        <li>Reward Points: <span class="list-value"> 200</span></li>
                //                        <li>Availability: <span class="list-value"> In Stock</span></li>
                //                    </ul>
                //                    <div class="price-block">
                //                        <span class="price-new">£73.79</span>
                //                        <del class="price-old">£91.86</del>
                //                    </div>
                //                    <div class="rating-widget">
                //                        <div class="rating-block">
                //                            <span class="fas fa-star star_on"></span>
                //                            <span class="fas fa-star star_on"></span>
                //                            <span class="fas fa-star star_on"></span>
                //                            <span class="fas fa-star star_on"></span>
                //                            <span class="fas fa-star "></span>
                //                        </div>
                //                        <div class="review-widget">
                //                            <a href="#">(1 Reviews)</a> <span>|</span>
                //                            <a href="#">Write a review</a>
                //                        </div>
                //                    </div>
                //                    <article class="product-details-article">
                //                        <h4 class="sr-only">Product Summery</h4>
                //                        <p>
                //                            Long printed dress with thin adjustable straps. V-neckline and wiring under
                //                            the Dust with ruffles
                //                            at the bottom
                //                            of the
                //                            dress.
                //                        </p>
                //                    </article>
                //                    <div class="add-to-cart-row">
                //                        <div class="count-input-block">
                //                            <span class="widget-label">Qty</span>
                //                            <input type="number" class="form-control text-center" value="1">
                //                        </div>
                //                        <div class="add-cart-btn">
                //                            <a href="#" class="btn btn-outlined--primary">
                //                                <span class="plus-icon">+</span>Add to Cart
                //                            </a>
                //                        </div>
                //                    </div>
                //                    <div class="compare-wishlist-row">
                //                        <a href="#" class="add-link"><i class="fas fa-heart"></i>Add to Wish List</a>
                //                        <a href="#" class="add-link"><i class="fas fa-random"></i>Add to Compare</a>
                //                    </div>
                //                </div>
                //            </div>
                //        </div>`

                $('#quickModal .product-details-modal').html(data)

            });

       

        $("#quickModal").modal("show")
    })
})