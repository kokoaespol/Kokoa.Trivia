package ec.edu.espol.kokoa.trivia.ui.theme

import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Shapes
import androidx.compose.material3.lightColorScheme
import androidx.compose.runtime.Composable
import androidx.compose.ui.unit.dp

val ColorScheme = lightColorScheme(
    background = KokoaLight,
    onBackground = KokoaBrown,
    primary = KokoaGreen,
    secondary = KokoaRed,
    tertiary = KokoaYellow,
)

@Composable
fun KokoaTriviaAppTheme(
    content: @Composable () -> Unit
) {
    MaterialTheme(
        colorScheme = ColorScheme,
        typography = Typography,
        content = content,
    )
}