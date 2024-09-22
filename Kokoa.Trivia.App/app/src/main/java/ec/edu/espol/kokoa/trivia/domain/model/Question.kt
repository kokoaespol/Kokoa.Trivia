package ec.edu.espol.kokoa.trivia.domain.model

data class Question(
    val id: Int,
    val title: String,
    val correctOption: Option,
    val options: List<Option>
)
