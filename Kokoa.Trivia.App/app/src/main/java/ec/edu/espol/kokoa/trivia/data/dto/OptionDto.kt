package ec.edu.espol.kokoa.trivia.data.dto

import androidx.core.content.contentValuesOf
import ec.edu.espol.kokoa.trivia.domain.model.Option

data class OptionDto(
    val id: Int,
    val content: String
)

fun OptionDto.toDomain(): Option = Option(
    id = id,
    content = content
)