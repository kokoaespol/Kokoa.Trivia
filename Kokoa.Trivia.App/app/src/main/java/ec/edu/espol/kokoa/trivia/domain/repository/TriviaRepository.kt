package ec.edu.espol.kokoa.trivia.domain.repository

import ec.edu.espol.kokoa.trivia.domain.model.Question
import ec.edu.espol.kokoa.trivia.domain.model.Topic

interface TriviaRepository {
    suspend fun getTopics(): List<Topic>
    suspend fun getQuestions(topicId: Int): List<Question>
}