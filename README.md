You are to develop a website for managing the Movies booking at the Cinema (local host) using all the
technologies taught during the lab sessions.

The user types and their permissions, respectfully: admin (managing the site) and users (booking a
movie and confirming the payment). Other users can be added with their permissions.

All of the types of users should get a separate page view according to their permissions.

Cinema is a slight version of the real Cinema Halls, which should manage movies, halls, payments, etc.

The following is a necessary list of requirements to be implemented in the project. You are responsible
of taking care of other possible constraints (e.g., different users are simultaneously trying to book the
same seat in the same hall while browsing the site from the same localholst, etc.). All the constraints,
which does not meet requirements, must give a relative error message.

Admin is responsible for:
-----------
 adding/removing movies
 managing the prices
 managing the movie halls (where each movie is demonstrated)
 managing the number of seats in each hall
 etc.

Users can:
---------
 choose a movie according to its date and time
 book an unoccupied seat
 change a seat (if it’s unoccupied) till the booking (that is, after the seat is chosen and payment is
processed, the seat cannot be checked).
 Make a payment
 Etc.

Movie gallery:
--------------
 has a list of movies with their posters, demonstration date and time, price and hall they are
shown
 has an age limitation
 a movie list can be ordered according to
o price increase
o price decrease
o most popular
o category
 Users can choose movies of a specific date/category/time/price range
 Movie list can be filtered to show only movies with the decreased price (on sale)
 Different movies can be shown at the same time but in different halls
 Each movie has a specific time and date. Show the last date the movie is demonstrated in the cinema.
 Movies can be shown for several times on the same day.

Buying a ticket:
-----------------
 Users can choose a seat for a specific movie at a specific date and time
 Show occupied/unoccupied seats
 A seat can be changed only before pressing “confirm” button in payment section.
 It’s impossible to choose an occupied seat
 It’s impossible to buy a ticket for a movie, which already started or ended
 Registered/unregistered users can buy a ticket
 If a movie has a price decrease, show the strikethrough previous price, and a new price.


Payment:
------------
 Managing a shopping cart
 Processing a payment (using SSL certificate – there are free ones online – is a bonus)
 No credit card must be stored in the database
 Bonus: the ability to pay with a PayPal (redirection to a PayPal site using its API)
 The user can choose to enter a movie to a shopping cart and process the payment from it or pay
directly
 Show notification message after the payment is accepted or failed. After that, a user is
redirected to Home page.
All data must be managed in the Database according to the user permission. For any violation, the
points will be taken.
