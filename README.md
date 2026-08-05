# ABC do Escorpião — Jogo Educativo 3D

**ABC do Escorpião** é um jogo sério educativo em ambiente 3D (*cartoon low poly*), desenvolvido na Unity Engine, com foco na conscientização infantil e comunitária sobre a prevenção de acidentes com escorpiões, condutas adequadas em caso de picada e importância da notificação aos órgãos de saúde pública.

O projeto foi desenvolvido no âmbito do Trabalho de Conclusão de Curso (PGC) do Curso de Ciência da Computação / Mestrado em Ciência da Computação.

---

## 🔗 Links e Acesso Rápido

* **Página Oficial e Download do Jogo:** [https://juniormaniusis.github.io/abc_escorpiao/](https://juniormaniusis.github.io/abc_escorpiao/)
* **Repositório do Código-Fonte:** [https://github.com/juniormaniusis/abc_escorpiao](https://github.com/juniormaniusis/abc_escorpiao)

---

## 📂 Estrutura do Código-Fonte Autoral (`src/`)

Neste repositório público estão disponibilizados **todos os scripts em C# de autoria própria e os bancos de dados de diálogos**, que sustentam a arquitetura e as regras de negócio do jogo:

```text
src/
├── Scripts/
│   ├── GameManager/         # SceneTransition (Hold/Release), IntroRevealGate, CursorManager, UIManager
│   ├── Player/              # PlayerMovement, PersonagemManager, PlayerStatus, PlayerScoreSaver
│   ├── Quest/               # IQuestItemSpawner, Factory Method de missões, ToysCarbinetInteractions
│   ├── Items/               # PickableItemSaver (Memento), InteractableDoor, Talkable, Apple
│   ├── SequencerCommands/   # Comandos customizados do Dialogue System (ex: troca de personagem em Timeline)
│   ├── Labirinto/           # Lógica do minigame do labirinto e cronômetro
│   ├── Root/                # GeradorMacasManager (A* NavMesh), AmbulanciaController, LuaFunctionsToDialogue
│   ├── Menu/                # Telas de interface, áudio e navegação
│   └── Tutorial/            # Disparadores de instruções em tempo de execução
└── Dialogos/
    ├── dialogo_escorpiao.csv # Banco de dados de falas, escolhas e perguntas educativas
    └── conversas.json        # Estruturas exportadas do Dialogue System
```

---

## 🛠️ Requisitos e Dependências para Executar

### 🕹️ Opção 1: Jogar o Executável Compilado (Usuários / Avaliadores)

Não é necessário instalar a Unity ou qualquer ambiente de desenvolvimento.

* **Sistema Operacional:** Windows 10 / 11 (64-bit).
* **Hardware Mínimo:** Processador Dual-Core 2.0 GHz, 4 GB RAM, Placa de vídeo integrada com suporte a DirectX 11.
* **Controles:** Teclado e Mouse (Setas / WASD para movimentação; Espaço / E para interação).
* **Download:** Baixe o arquivo `.zip` da versão compilada na [Página do Projeto](https://juniormaniusis.github.io/abc_escorpiao/).

---

### 💻 Opção 2: Reconstruir ou Executar no Unity Editor (Desenvolvedores)

Para importar o código-fonte autoral e executar o projeto no Unity Editor, são necessários os seguintes ambientes e dependências:

#### 1. Versão da Engine
* **Unity 2022.3 LTS** (ou versão mais recente da linha 2022.3).

#### 2. Pacotes Gratuitos do Unity Package Manager (UPM)
Estes pacotes são instalados gratuitamente via Package Manager nativo da Unity:
* `com.unity.inputsystem` (**Input System**) — Gerenciamento de entradas de teclado/gamepad.
* `com.unity.cinemachine` (**Cinemachine**) — Câmeras virtuais e transições dinâmicas.
* `com.unity.timeline` (**Timeline**) — Sequenciamento de cutscenes e animações.
* `com.unity.textmeshpro` (**TextMesh Pro**) — Renderização de fontes e textos de interface.
* `com.unity.ai.navigation` (**AI Navigation / NavMesh**) — Navegação e cálculo de rotas $A^*$.

#### 3. Dependências Comerciais de Terceiros (Assets da Unity Asset Store)
O projeto utiliza os seguintes pacotes de terceiros para interface e feedbacks. **Se você desejar abrir e compilar o projeto completo a partir do zero**, precisará importar as licenças desses pacotes:
* **[Dialogue System for Unity](https://assetstore.unity.com/packages/tools/game-toolkits/dialogue-system-for-unity-11672)** (Pixel Crushers) — Gerenciamento das árvores de conversas e variáveis Lua.
* **[Feel](https://assetstore.unity.com/packages/tools/utilities/feel-183270)** (More Mountains) — Efeitos de juiciness, feedbacks sonoros e visuais.
* **[Polyperfect Low Poly Assets](https://assetstore.unity.com/packages/3d/environments/landscapes/low-poly-ultimate-pack-148784)** — Modelos 3D de cenários, objetos domésticos e personagens.

---

## 📜 Nota de Licenciamento e Direitos Autorais

Conforme os Termos de Serviço da **Unity Asset Store (EULA)**, os modelos 3D, efeitos e bibliotecas comerciais de terceiros listados acima **foram omitidos desta distribuição de código-fonte aberto** para proteger as licenças dos respectivos criadores.

Todo o **código-fonte C# autoral, algoritmos de jogo, serializadores e bancos de dados de diálogos** contidos na pasta `src/` estão licenciados sob a **Licença MIT**, permitindo livre consulta, auditoria acadêmica e reaproveitamento do conhecimento técnico.

---

## ✉️ Contato e Autoria

* **Autor:** Junior Maniusis
* **Projeto:** ABC do Escorpião — Pesquisa e Desenvolvimento de Jogos Educativos.
