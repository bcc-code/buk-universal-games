<template>
  <AdminPageLayout>
    <div class="flex">
      <nav
        class="text-dark mr-4 mb-5 bg-ice-blue cursor-pointer rounded-md hover:bg-vanilla text-dark-blue btn"
        @click="$router.back()"
      >
        <div class=" ">
          <ArrowLeftIcon class="h-5" />
        </div>
      </nav>
    </div>
    <div class="toast toast-center toast-top" v-if="showErrorPopup">
      <div class="alert alert-error block">{{ popupErrorMessage }}</div>
    </div>
    <div class="toast toast-center toast-top" v-if="showSuccess">
      <div class="alert alert-success block">Lagret</div>
    </div>

    <MatchListItem
      class="mb-5"
      v-if="match && match.team1 && match.team2"
      :key="match.matchId"
      :team1="match.team1"
      :team2="match.team2"
      :team1result="match.team1Result ?? null"
      :team2result="match.team2Result ?? null"
      :start="match.start ?? ''"
      :winner="match.winner"
      :gameType="game?.gameType ?? ''"
      addOn=""
      gameAddOn=""
    />

    <div class="rounded-md bg-white p-4 mb-4 bg-green-50 italic">
      Spør hvilket lag de er og sørg for at det er riktig lag på rett plass.
    </div>

    <template v-if="game?.gameType === 'iron_grip'">
      <div class="rounded-md bg-white p-4 mb-4 bg-amber-50">
        <span class="font-bold">Manus:</span>
        <br />
        Velkommen til jerngrepet.
        <ul class="list-disc list-inside">
          <li>Målet med posten er å samle mest mulig "henge-tid" i løpet av 10 minutter.</li>
          <li>Alle kan være med, men kun 9 stk henger om gangen.</li>
          <li>Laget samler "henge-tid" når 9 stk fra laget henger samtidig fra stillaset.</li>
          <li>Laget har 10 minutter på gjennomføre posten.</li>
          <li>Når det er færre en 9 deltakere som henger samtidig, vil tiden som samles for "henge-tid" stoppes, frem til det er 9 deltakere som henger samtidig igjen. Da startes tiden igjen.</li>
          <li>Deltakere som ikke klarer å henge lenger må stille seg bakerst i køen på siden av stillaset.</li>
          <li>Det er kun deltakerne som henger som har lov til å være oppe på stillaset, resten av laget må stå i kø, på siden av stillaset (peker mot riktig side).</li>
          <li>Ved juksing vil laget få 10 sek i fratrekk fra oppsamlet tid.</li>
          <li>Deltakerne kan kun henge etter hendene sin, uten støtte fra beina, andre deltakere eller gjenstander.</li>
          <li>Dommeren bestemmer om noe er juks eller ikke, men alt som gjør at deltakere får hjelp til å henge utenom egen styrke defineres som juks.</li>
        </ul>
      </div>

      <div class="rounded-md bg-white p-4 mb-4 bg-blue-50">
        <span class="font-bold">Til dommere:</span>
        <ul class="list-disc list-inside">
          <li>Ha en tidtaker som teller ned fra 10 minutter klar, som settes i gang ved starten av konkurransen.</li>
          <li>Ha en tidtaker som brukes som oppsamlet "henge-tid", denne stoppes hver gang det er færre enn 9 deltakere som henger fra stillaset samtidig, og startes igjen når 9 stk henger samtidig.</li>
          <li>En deltaker kan delta så mange ganger en vil.</li>
          <li>Laget bestemmer selv hvilke deltakere de har lyst å bruke til å henge, og dette kan også endres underveis.</li>
          <li>Deltakerne skal henge etter hendene strakt ned fra stillaset, og kan ikke bruke bein, andre deltakere, gjenstander o.l for å få hjelp.</li>
          <li>En dommer følger med på juks. VIKTIG: påpek til laget at dette er juks, og så noter ned.</li>
          <li>Si i fra på følgende tider: Nå er det 5 minutter igjen, halvveis! Nå er det 2 minutter igjen, Nå er det 1 minutt igjen!</li>
        </ul>
        <br />
        <span>Ved spillets slutt:</span>
        <ul class="list-disc list-inside">
          <li>Registrer "henge-tiden" i appen. TODO FOR SIGURD</li>
          <li>Sikkerhetsrutine: Gå over stillaset å sjekk at alt sitter godt fast etter hver runde.</li>
        </ul>
      </div>

      <div class="rounded-md bg-white p-4 mb-4 bg-red-50">
        <span class="font-bold">Helse, miljø og sikkerhet:</span>
        <ul class="list-disc list-inside">
          <li>Fare for fall fra stillaset, overtråkk ved fall, blemmer i hendene.</li>
          <li>Der posten har asfalt som underlag vil det være matter.</li>
        </ul>
      </div>
    </template>

    <template v-else-if="game?.gameType === 'land_water_beach'">
      <div class="rounded-md bg-white p-4 mb-4 bg-amber-50">
        <span class="font-bold">Manus:</span>
        <br />
        Velkommen til Land, vann, strand.
        <ul class="list-disc list-inside">
          <li>På denne posten skal dere konkurrere mot et annet lag om å reagere raskest mulig og flytte dere mellom tre soner på tre ulike signaler.</li>
          <li>Ti personer fra hvert lag starter i den midterste sonen, strand.</li>
          <li>De tre signalene dere skal reagere på er:
            <ul>
              <li>Rødt signal betyr at dere skal bevege dere til den bakerste sonen, land.</li>
              <li>Gult signal er sonen i midten, strand.</li>
              <li>Blått signal er sonen foran, vann.</li>
            </ul>
          </li>
          <li>Det laget hvor alle ti spillere beveger seg først til riktig sone, får ett poeng. Hvis begge lag er like raske, får begge lag ett poeng.</li>
          <li>Spillerne skal bli stående i den sonen de flytter seg til.</li>
          <li>Det vises tre signaler, før de ti spillerne byttes ut med ti nye spillere. Og dette skal gjennomføres totalt tre omganger.</li>
          <li>Dere får poeng etter hvor mange runder dere vinner.</li>
          <li>Er oppgaven forstått?</li>
          <li>Da kan ti spillere starte i den midterste sonen, og resten av laget kan stå der, og være klare for å bytte ut de ti spillerne. (peker der det er best at resten av laget står)</li>
          <li>Da setter vi i gang. Klar, ferdig, gå!</li>
        </ul>
      </div>

      <div class="rounded-md bg-white p-4 mb-4 bg-blue-50">
        <span class="font-bold">Til dommere:</span>
        <ul class="list-disc list-inside">
          <li>Passe på at resten av laget står et passende sted, og er klare til å bytte inn ti nye spillere for hver omgang.</li>
          <li>Passe på at tiden ikke overstiger 15 minutter totalt på posten.</li>
          <li>Hoveddommer viser poeng på poengtavle.</li>
          <li>To linjedommere med flagg, tar rask avgjørelse om hvem som får poeng.</li>
          <li>Personen i signalboksen må variere signal og tiden mellom signalene.</li>
        </ul>
        <br />
        <span>Ved spillets slutt:</span>
        <ul class="list-disc list-inside">
          <li>Sørge for at sonene fortsatt er tydelig oppmerket.</li>
        </ul>
      </div>

      <div class="rounded-md bg-white p-4 mb-4 bg-red-50">
        <span class="font-bold">Helse, miljø og sikkerhet:</span>
        <ul class="list-disc list-inside">
          <li>Det er liten risiko knyttet til denne posten, eventuelt kan snubling, overtråkk eller fall forekomme. Vær obs på dette.</li>
        </ul>
      </div>
    </template>

    <template v-else-if="game?.gameType === 'labyrinth'">
      <div class="rounded-md bg-white p-4 mb-4 bg-amber-50">
        <span class="font-bold">Manus:</span>
        <br />
        Velkommen til Labyrinten.
        <ul class="list-disc list-inside">
          <li>På denne posten må dere samarbeide for å få en kule gjennom en labyrint.</li>
          <li>Inntil 12 spillere samarbeider om gangen, og dere har maks ti minutter på å gjennomføre posten.</li>
          <li>Det er to checkpoints på labyrinten hvor de 12 spillerne skal byttes ut med 12 nye spillere.</li>
          <li>Det er ikke tillat å berøre kulen med hendene når den er på brettet, eller kaste den opp.</li>
          <li>Hvis kulen faller ned i et hull, må en av spillerne plukke den opp og plassere den tilbake ved startpunktet eller ved forrige checkpoint, men det er kun tillat å være under labyrinten når kulen faller gjennom et hull.</li>
          <li>Dere får to poeng for hvert checkpoint dere passerer, og flere poeng jo raskere dere gjennomfører.</li>
          <li>Når labyrinten er fullført eller tiden er ute kan dere plassere labyrinten rolig ned på bakken.</li>
          <li>Spillet starter når en dommer plasserer kulen på startpunktet.</li>
          <li>Er oppgaven forstått?</li>
          <li>Da kan 12 spillere gjøre seg klare til å starte på labyrinten, og resten av laget kan stå her borte (peke dit det er gunstig at resten av laget står).</li>
          <li>12 nye spillere må stå klare til å ta over ved checkpointene.</li>
          <li>Da setter vi i gang. Klar, ferdig, gå!</li>
        </ul>
      </div>

      <div class="rounded-md bg-white p-4 mb-4 bg-blue-50">
        <span class="font-bold">Til dommer:</span>
        <ul class="list-disc list-inside">
          <li>Få kontroll på folkemengder, og plassere tilskuere best mulig.</li>
          <li>Stå klar til å starte nedtelling på 10 minutter når laget starter.</li>
          <li>Sørg for at alle spillerne bytter med ny spiller ved passering av checkpoints.</li>
          <li>Følge med på at deltakerne ikke bryter noen av reglene.</li>
          <li>Stoppe spillet etter 10 minutter, hvis ikke laget allerede har gjennomført oppgaven.</li>
        </ul>
        <br />
        <span>Ved spillets slutt:</span>
        <ul class="list-disc list-inside">
          <li>Samle alle kulene som har vært i spill.</li>
          <li>Gjør klar for nye grupper!</li>
        </ul>
      </div>

      <div class="rounded-md bg-white p-4 mb-4 bg-red-50">
        <span class="font-bold">Helse, miljø og sikkerhet:</span>
        <ul class="list-disc list-inside">
          <li>Risiko knyttet til denne posten er at deltakerne kan miste labyrinten på føttene sine. Hvis ballen faller gjennom et hull, må deltakerne være forsiktige når de plukker den opp, slik at man ikke får labyrinten over seg. Flis eller blemmer i håndflaten kan forekomme.</li>
        </ul>
      </div>
    </template>

    <template v-else-if="game?.gameType === 'human_shuffleboard'">
      <div class="rounded-md bg-white p-4 mb-4 bg-amber-50">
        <span class="font-bold">Manus:</span>
        <br />
        Velkommen til Hamsterhjulet.
        <ul class="list-disc list-inside">
          <li>På denne posten skal dere konkurrere mot et annet lag i en stafett. Laget som vinner og har best tid får flest poeng.</li>
          <li>Lagene skal på kortest mulig tid komme til den ene siden og tilbake.</li>
          <li>Et lag må ha 14 deltakere i hjulet samtidig. Det skal gjennom hele løypen være 14 deltakere inne i hjulet.</li>
          <li>Hele laget skal samlet utføre stafetten. Laget må gå innenfor teppet med teppet over hodet gjennom hele løypa.</li>
          <li>Såfremt laget har nok deltakere kan personene i hjulet byttes ut med 14 nye personer når laget er halvveis.</li>
          <li>Laget må over streken med hele hjulet før laget kan bytte deltakerne.</li>
          <li>Det laget som er først tilbake med hele laget over startstreken vinner. Tiden vil også bli registrert, så her gjelder det for begge lag å gjøre det raskest mulig selv om en ikke kommer først i mål.</li>
          <li>Dere får 2 minutter på å gjøre dere klare.</li>
          <li>Oppgaven begynner på mitt (dommerens) signal.</li>
          <li>Er oppgaven forstått?</li>
          <li>Da kan lagene gjøre seg klare ved startstreken. Gruppen som skal ta over kan stelle seg på andre siden av banen. Nedtelling på 2 minutter starter nå.</li>
          <li>Tiden er nå ute. Er alle lag klare?</li>
        </ul>
      </div>

      <div class="rounded-md bg-white p-4 mb-4 bg-blue-50">
        <span class="font-bold">Til dommer:</span>
        <ul class="list-disc list-inside">
          <li>To dommere plasseres på hver sin side av lagene og registrerer på telefon hvor ofte lagene tråkker utenfor båndet og hver gang laget ikke har stoffet over hodet.</li>
          <li>Sørg for at deltakere og publikum har tydelige plasseringer ovenfor spillbrettet med tanke på spillopplevelse, innsyn og engasjement.</li>
        </ul>
        <br />
        <span>Ved spillets slutt:</span>
        <ul class="list-disc list-inside">
          <li>Samle dukene og fordel de klart til neste lag som kommer.</li>
          <li>Rett opp startstrek og andre gjerder om nødvendig.</li>
          <li>Gjør klar for ny gruppe!</li>
        </ul>
      </div>

      <div class="rounded-md bg-white p-4 mb-4 bg-red-50">
        <span class="font-bold">Helse, miljø og sikkerhet:</span>
        <ul class="list-disc list-inside">
          <li>Vær oppmerksom på at hamsterhjulet innebærer at mange personer skal gå i et høyt tempo samtidig og tett på hverandre som kan medføre at noen kan falle. Dette anses som en middels risiko.</li>
        </ul>
      </div>
    </template>

    <template v-else-if="game?.gameType === 'mastermind'">
      <div class="rounded-md bg-white p-4 mb-4 bg-amber-50">
        <span class="font-bold">Manus:</span>
        <br />
        Velkommen til Mastermind.
        <ul class="list-disc list-inside">
          <li>Målet med denne posten er å finne ut hvilken fargekode som befinner seg i kofferten.</li>
          <li>Dere får 8 runder og en makstid på 10 minutter på å løse oppgaven.</li>
          <li>Dere får flere poeng jo færre runder dere bruker på å komme til riktig kode.</li>
          <li>Dere skal nå plukke ut 5 Codebreakers.</li>
          <li>Disse 5 har i hovedoppgave å løse oppgaven.</li>
          <li>Resten får utdelt hver sin farge (Durag - hodeplagg) som representerer en spillbrikke som Codebreakers kan plassere ut på brettet.</li>
          <li>Alle deltakere som er spillbrikker må ha hodeplagget på hodet til en hver tid under spillet. (Ha med/på duragen for å vise frem)</li>
          <li>Codebreakers henter de spillbrikkene (deltakere med hodeplagg) som de ønsker å bruke og plasserer dem på en sort spillbrikke på spillbrettet.</li>
          <li>Når Codebreakers har avgitt sitt svar, vil en dommer plassere ut «svar-pinner» som forteller Codebreakers hvor nærme de er svaret.
            <ul>
              <li>Rød pinne = riktig farge på riktig plass</li>
              <li>Hvit pinne = riktig farge på feil plass</li>
            </ul>
            (Ha med pinnene i hånden og vis de frem)
          </li>
          <li>Når Codebreakers har gjettet riktig fargekode, vil en dommer gå frem og åpne boksen.</li>
          <li>Resten av laget står på siden av spillbrettet.</li>
          <li>Er oppgaven forstått?</li>
          <li>Da kan Codebreakers gå til den dommeren der (peker og viser).</li>
          <li>Alle de som skal være spillbrikker, kan stå over der (peker og viser).</li>
          <li>Da setter vi i gang. Klar, ferdig, gå!</li>
        </ul>
      </div>

      <div class="rounded-md bg-white p-4 mb-4 bg-blue-50">
        <span class="font-bold">Til dommer:</span>
        <ul class="list-disc list-inside">
          <li>Sørg for at publikum, spillbrikker og codebreakers har tydelige plasseringer ovenfor spillbrettet med tanke på spillopplevelse, innsyn og engasjement.</li>
        </ul>
        <br />
        <span>Ved spillets slutt:</span>
        <ul class="list-disc list-inside">
          <li>Samle inn alle hodeplagg.</li>
          <li>Samle inn svar-pinner.</li>
          <li>Endre fargekode i boksen.</li>
          <li>Juster spillbrettet om nødvendig.</li>
          <li>Gjør klar for nye grupper!</li>
        </ul>
      </div>

      <div class="rounded-md bg-white p-4 mb-4 bg-red-50">
        <span class="font-bold">Helse, miljø og sikkerhet:</span>
        <ul class="list-disc list-inside">
          <li>Risiko knyttet til denne posten er at deltakere kan snuble i spillbrikkene på spillbrettet. Dette anses som svært lav til lav risiko.</li>
        </ul>
      </div>
    </template>

    <template v-else>
      Noe gikk galt. Ingen spill valgt.
    </template>


    <div class="teams">
      <div
        :class="{
          teamresult: true,
        }"
      >
        <p>{{ match?.team1 }}</p>
        <p v-if="match?.team1Result">
          <strong>Score:</strong> {{ match?.team1Result }}
        </p>
        <div>
          <input
            class="bg-slate-100"
            type="number"
            v-model="team1Result"
            :placeholder="'Input result'"
          />
          <button
            class="btn btn-success btn-blank ml-3"
            @click="confirmTeamResult(match?.team1Id, team1Result)"
            :disabled="confirmDisabled"
          >
            Save
          </button>
        </div>
      </div>
      <div
        v-if="match?.team1Id !== match?.team2Id"
        :class="{
          teamresult: true,
        }"
      >
        <p>{{ match?.team2 }}</p>
        <p v-if="match?.team2Result">
          <strong>Score:</strong> {{ match?.team2Result }}
        </p>

        <div>
          <input
            class="bg-slate-100"
            type="number"
            v-model="team2Result"
            :placeholder="'Input result'"
          />
          <button
            class="btn btn-success btn-blank ml-3"
            @click="confirmTeamResult(match?.team2Id, team2Result)"
            :disabled="confirmDisabled"
          >
            Save
          </button>
        </div>
      </div>
    </div>
  </AdminPageLayout>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import { useRouter, useRoute } from 'vue-router';
import { useAdminMatches, useGames, useConfirmTeamResult } from '@/hooks/hooks';
import AdminPageLayout from '@/components/AdminPageLayout.vue';
import { ArrowLeftIcon } from '@heroicons/vue/24/solid';
import { useStore } from 'vuex';
import MatchListItem from '@/components/MatchListItem.vue';

const route = useRoute();
const router = useRouter();
const matchId = Number(route.params.matchId as string);
const leagueId = useStore().state.adminLeagueSelected;

const { data: matches } = useAdminMatches(leagueId);
const { data: games } = useGames();
const { mutate: confirmResult, isPending } = useConfirmTeamResult();

const isChangingScore1 = ref(false);
const isChangingScore2 = ref(false);
const popupErrorMessage = ref<string | null>(null);
const showErrorPopup = computed(() => !!popupErrorMessage.value);

const match = computed(() =>
  matches.value?.find((m: any) => m.matchId == matchId),
);
const game = computed(() =>
  games.value?.find((g: any) => g.id == match.value?.gameId),
);
const team1Result = ref<number | null>(match.value?.team1Result ?? null);
const team2Result = ref<number | null>(match.value?.team2Result ?? null);

const confirmDisabled = computed(() => isPending.value);

const showSuccess = ref<boolean>(false);

const showError = () => {
  popupErrorMessage.value =
    'Submitting failed, please check connection and try again';
  setTimeout(() => {
    popupErrorMessage.value = null;
  }, 5000);
};

const showSuccessToast = () => {
  showSuccess.value = true;
  setTimeout(() => {
    showSuccess.value = false;
  }, 3000);
};

const confirmTeamResult = async (
  teamId: number | undefined,
  result: number | null,
) => {
  if (!match.value || !teamId) {
    alert('noe gikk galt');
    return;
  }
  if (
    typeof result === 'number' &&
    Number.isInteger(result) &&
    result >= 1 &&
    result <= 20
  ) {
    confirmResult(
      { matchId, teamId, result },
      {
        onSuccess: () => {
          if (teamId === match.value?.team1Id) {
            isChangingScore1.value = false;
          } else {
            isChangingScore2.value = false;
          }
          showSuccessToast();
        },
        onError: () => {
          showError();
        },
      },
    );
  } else {
    alert('Please enter a whole number between 1 and 20');
  }
};

if (!matchId) {
  router.back();
}
</script>

<style scoped>
nav {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

div.teams {
  display: grid;
  gap: 1em;
  margin: 0 0 2em 0;
  padding: 0;
}

div.teamresult {
  padding: 1em;
  background-color: #fff;
  border-radius: 1em;
  position: relative;
  display: grid;
  grid-template-rows: auto auto auto;
}

div.teamresult .tag {
  position: absolute;
  top: 1em;
  right: 1em;
  text-transform: uppercase;
  border-radius: 4px;
  padding: 0.25em 0.5em;
}

div.teamresult .tag {
  color: #000;
  background-color: var(--red);
}

div.teamresult.winner .tag {
  color: #000;
  background-color: var(--green);
}
</style>
