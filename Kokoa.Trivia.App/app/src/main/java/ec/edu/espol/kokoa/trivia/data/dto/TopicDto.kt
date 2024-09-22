package ec.edu.espol.kokoa.trivia.data.dto

import ec.edu.espol.kokoa.trivia.domain.model.Topic

data class TopicDto(
    val id: Int,
    val name: String
)

fun TopicDto.toDomain(): Topic = Topic(
    id = id,
    name = name,
)
