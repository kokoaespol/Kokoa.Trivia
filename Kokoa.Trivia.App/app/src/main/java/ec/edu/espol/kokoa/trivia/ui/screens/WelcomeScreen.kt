package ec.edu.espol.kokoa.trivia.ui.screens

import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.height
import androidx.compose.material3.Button
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.scale
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.navigation.NavController
import ec.edu.espol.kokoa.trivia.R
import ec.edu.espol.kokoa.trivia.ui.Screen

@Composable
fun WelcomeScreen(
    navController: NavController,
    modifier: Modifier = Modifier
) {
    Column(
        modifier = modifier.fillMaxSize(),
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.Center
    ) {
        Text(
            text = "KOKOA TRIVIA",
            fontSize = 32.sp,
            fontWeight = FontWeight.Bold
        )
        Spacer(
            modifier = Modifier.height(100.dp)
        )
        Image(
            painter = painterResource(R.drawable.splash),
            contentDescription = "splash",
            modifier = Modifier.scale(2f)
        )
        Spacer(
            modifier = Modifier.height(100.dp)
        )
        Button(
            onClick = { navController.navigate(Screen.Topics) },
        ) {
            Text(
                text = "Empezar!",
                fontSize = 26.sp
            )
        }
    }
}
