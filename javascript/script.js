const movies = [
  { title: "Inception", genres: ["Sci-Fi", "Drama"], description: "Dreams within dreams—what is reality?" },
  { title: "The Matrix", genres: ["Sci-Fi", "Action"], description: "Hacker joins a rebellion against simulated reality." },
  { title: "The Shawshank Redemption", genres: ["Drama"], description: "A man finds hope and friendship in prison." },
  { title: "Mad Max: Fury Road", genres: ["Action", "Sci-Fi"], description: "Post-apocalyptic high-octane survival." },
  { title: "John Wick", genres: ["Action"], description: "Revenge thriller with sleek, brutal combat." },
  { title: "Get Out", genres: ["Horror", "Drama"], description: "A chilling, smart social horror story." },
  { title: "The Conjuring", genres: ["Horror"], description: "Paranormal investigators face evil spirits." },
  { title: "Superbad", genres: ["Comedy"], description: "Coming-of-age chaos and teenage antics." },
  { title: "Forrest Gump", genres: ["Drama", "Comedy"], description: "A kind man's accidental journey through history." },
  { title: "Step Brothers", genres: ["Comedy"], description: "Two childish men become stepbrothers and roommates." },
  { title: "The Mask", genres: ["Comedy", "Action"], description: "A magical mask transforms a man into a cartoon hero." },
  { title: "A Quiet Place", genres: ["Horror", "Sci-Fi"], description: "Silence is survival in a world of monsters." },
  { title: "The Pursuit of Happyness", genres: ["Drama"], description: "A father struggles to build a better life." },
  { title: "Die Hard", genres: ["Action"], description: "A lone cop takes on terrorists in a skyscraper." },
  { title: "Interstellar", genres: ["Sci-Fi", "Drama"], description: "Humanity searches for hope beyond the stars." }
];

document.getElementById("recommendBtn").addEventListener("click", () => {
  const selectedGenres = Array.from(document.querySelectorAll(".genre-box:checked"))
    .map(box => box.value);
  const resultsDiv = document.getElementById("results");
  resultsDiv.innerHTML = "";

  if (selectedGenres.length === 0) {
    resultsDiv.textContent = "Please select at least one genre.";
    return;
  }

  const recommendations = movies.filter(movie =>
    selectedGenres.every(g => movie.genres.includes(g))
  );

  if (recommendations.length === 0) {
    resultsDiv.innerHTML = "<p>No movies match all selected genres.</p>";
  } else {
    recommendations.forEach((movie, i) => {
      const movieDiv = document.createElement("div");
      movieDiv.innerHTML = `<strong>${i + 1}. ${movie.title}</strong><br><em>${movie.genres.join(", ")}</em><br>${movie.description}<br><br>`;
      resultsDiv.appendChild(movieDiv);
    });
  }
});
