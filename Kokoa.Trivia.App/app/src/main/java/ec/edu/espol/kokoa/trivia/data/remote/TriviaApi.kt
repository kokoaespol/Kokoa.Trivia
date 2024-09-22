package ec.edu.espol.kokoa.trivia.data.remote

import ec.edu.espol.kokoa.trivia.data.dto.QuestionDto
import ec.edu.espol.kokoa.trivia.data.dto.TopicDto
import retrofit2.Response
import retrofit2.http.GET
import retrofit2.http.Path

interface TriviaApi {
    @GET("api/topics")
    suspend fun getTopics(): Response<List<TopicDto>>

    @GET("api/topics/{topicId}/questions")
    suspend fun getQuestions(@Path("topicId") topicId: Int): Response<List<QuestionDto>>
}