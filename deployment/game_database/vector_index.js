// Create the following atlas vector search index:

// ==========================
// named: vector_index
// collection: recordings
// ==========================
{
  "fields": [
    {
      "path": "Player.Nickname",
      "type": "filter"
    },
    {
      "numDimensions": 598,
      "path": "similarity_vector",
      "similarity": "euclidean",
      "type": "vector"
    },
    {
      "numDimensions": 9,
      "path": "stats_vector",
      "similarity": "euclidean",
      "type": "vector"
    },
    {
      "numDimensions": 589,
      "path": "speed_vector",
      "similarity": "euclidean",
      "type": "vector"
    },
    {
      "numDimensions": 588,
      "path": "accel_vector",
      "similarity": "euclidean",
      "type": "vector"
    },
    {
      "numDimensions": 60,
      "path": "ratios_vector",
      "similarity": "euclidean",
      "type": "vector"
    },
    {
      "numDimensions": 60,
      "path": "scoreCumulative_vector",
      "similarity": "euclidean",
      "type": "vector"
    },
    {
      "numDimensions": 60,
      "path": "average_score_per_bullet_vector",
      "similarity": "euclidean",
      "type": "vector"
    },
    {
      "numDimensions": 60,
      "path": "average_damage_per_bullet_vector",
      "similarity": "euclidean",
      "type": "vector"
    },
    {
      "numDimensions": 60,
      "path": "average_pellets_per_bullet_vector",
      "similarity": "euclidean",
      "type": "vector"
    },
  ]
}