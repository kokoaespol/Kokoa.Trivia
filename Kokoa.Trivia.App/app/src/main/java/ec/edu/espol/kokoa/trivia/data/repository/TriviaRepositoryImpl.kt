package ec.edu.espol.kokoa.trivia.data.repository

import ec.edu.espol.kokoa.trivia.data.dto.toDomain
import ec.edu.espol.kokoa.trivia.data.remote.TriviaApi
import ec.edu.espol.kokoa.trivia.domain.model.Question
import ec.edu.espol.kokoa.trivia.domain.model.Topic
import ec.edu.espol.kokoa.trivia.domain.repository.TriviaRepository

class TriviaRepositoryImpl(
    val api: TriviaApi
): TriviaRepository {
    override suspend fun getTopics(): List<Topic> {
        return api.getTopics().body()?.let { topics ->
            topics.map {
                it.toDomain()
            }
        } ?: emptyList()
    }

    override suspend fun getQuestions(topicId: Int): List<Question> {
        return api.getQuestions(topicId).body()?.let { questions ->
            questions.map {
                Question(
                    id = it.id,
                    title = it.title,
                    correctOption = it.correctOption.toDomain(),
                    options = it.options.map { option -> option.toDomain() }
                )
            }
        } ?: emptyList()
    }
}